// REQ-3 / AC-3: valida o script real da página, sem pacotes ou servidor.
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');
const assert = require('node:assert/strict');
const { test } = require('node:test');

const html = fs.readFileSync(path.join(__dirname, '../app/index.html'), 'utf8');
const script = html.match(/<script>([\s\S]*?)<\/script>/)[1];

function setup(fetchReply) {
  let listener;
  const requests = [];
  const ids = ['discount-form', 'submit', 'status', 'error', 'result', 'original', 'percentage', 'total'];
  const elements = Object.fromEntries(ids.map(id => [id, {
    hidden: true, disabled: false, textContent: '', attributes: {},
    setAttribute(key, value) { this.attributes[key] = value; },
    removeAttribute(key) { delete this.attributes[key]; }
  }]));
  const form = elements['discount-form'];
  form.elements = { amount: { value: '100' }, discount: { value: '10' } };
  form.reportValidity = () => true;
  form.addEventListener = (_, fn) => { listener = fn; };
  vm.runInNewContext(script, {
    document: { getElementById: id => elements[id] },
    Intl, Number, JSON, Error, TypeError, SyntaxError,
    fetch: async (url, options) => {
      requests.push({ url, ...options });
      return fetchReply();
    }
  });
  return { elements, form, requests, submit: () => listener({ preventDefault() {} }) };
}

const success = () => ({ ok: true, json: async () => ({ original: '100.00', discount: '10', total: '90.00' }) });

test('script compila e campos possuem labels', () => {
  new vm.Script(script);
  assert.match(html, /<label for="amount">/);
  assert.match(html, /<label for="discount">/);
});

test('envia contrato numérico e apresenta resultado em BRL', async () => {
  const page = setup(success);
  await page.submit();
  assert.equal(page.requests[0].url, '/api/discount');
  assert.equal(page.requests[0].method, 'POST');
  assert.deepEqual(JSON.parse(page.requests[0].body), { amount: 100, discount: 10 });
  assert.match(page.elements.total.textContent, /R\$\s*90,00/);
  assert.equal(page.elements.percentage.textContent, '10%');
  assert.equal(page.elements.result.hidden, false);
  assert.equal(page.elements.submit.disabled, false);
});

test('carregamento desabilita botão e bloqueia segunda chamada', async () => {
  let finish;
  const page = setup(() => new Promise(resolve => { finish = resolve; }));
  const pending = page.submit();
  assert.equal(page.elements.submit.disabled, true);
  assert.equal(page.form.attributes['aria-busy'], 'true');
  assert.equal(page.elements.status.textContent, 'Calculando…');
  await page.submit();
  assert.equal(page.requests.length, 1);
  finish(success());
  await pending;
  assert.equal(page.elements.submit.disabled, false);
  assert.equal(page.form.attributes['aria-busy'], undefined);
});

test('erro de API oculta resultado anterior e restaura botão', async () => {
  const page = setup(() => ({ ok: false, json: async () => ({ error: 'Desconto inválido' }) }));
  page.elements.result.hidden = false;
  await page.submit();
  assert.equal(page.elements.error.textContent, 'Desconto inválido');
  assert.equal(page.elements.error.hidden, false);
  assert.equal(page.elements.result.hidden, true);
  assert.equal(page.elements.submit.disabled, false);
});

test('valores inválidos não chamam API', async () => {
  for (const [amount, discount] of [['0', '10'], ['-1', '10'], ['100', '21'], ['100', '-1'], ['NaN', '10']]) {
    const page = setup(success);
    page.form.elements.amount.value = amount;
    page.form.elements.discount.value = discount;
    await page.submit();
    assert.equal(page.requests.length, 0);
    assert.equal(page.elements.error.hidden, false);
  }
});

test('falha de transporte e resposta fora do contrato têm erro legível', async () => {
  const offline = setup(() => { throw new TypeError('network'); });
  await offline.submit();
  assert.match(offline.elements.error.textContent, /Verifique se a aplicação está em execução/);
  const invalid = setup(() => ({ ok: true, json: async () => ({ original: '100', discount: '10', total: 'oops' }) }));
  await invalid.submit();
  assert.match(invalid.elements.error.textContent, /resultado inválido/);
  assert.equal(invalid.elements.result.hidden, true);
});

test('rejeita perda de precisão monetária antes de enviar números JSON', async () => {
  const page = setup(success);
  page.form.elements.amount.value = '100000000000000.01';
  await page.submit();
  assert.equal(page.requests.length, 0);
  assert.match(page.elements.error.textContent, /precisão/);
  assert.equal(page.elements.error.hidden, false);
});

test('dinheiro da resposta é apresentado exatamente sem conversão Number', async () => {
  const page = setup(() => ({ ok: true, json: async () => ({
    original: '100000000000000.01', discount: '0', total: '100000000000000.01'
  }) }));
  await page.submit();
  assert.equal(page.elements.original.textContent, 'R$ 100.000.000.000.000,01');
  assert.equal(page.elements.total.textContent, 'R$ 100.000.000.000.000,01');
});

test('notação científica e zeros finais não provocam falsa perda de precisão', async () => {
  for (const [amount, discount] of [['1e2', '1e1'], ['100.0000', '10.00'], ['.0100', '0.000'], ['1e-7', '2e1'], ['1e20', '0']]) {
    const page = setup(success);
    page.form.elements.amount.value = amount;
    page.form.elements.discount.value = discount;
    await page.submit();
    assert.equal(page.requests.length, 1, `${amount}, ${discount}`);
    assert.deepEqual(JSON.parse(page.requests[0].body), { amount: Number(amount), discount: Number(discount) });
  }
});
