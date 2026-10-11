'use strict';
const $ = id => document.getElementById(id);
const text = value => typeof value === 'string' ? value : JSON.stringify(value ?? null, null, 2);
function money(value) {
  const raw = String(value);
  if (!/^\d+(?:\.\d{1,2})?$/.test(raw)) throw new Error('A API retornou um valor monetário inválido.');
  const [whole, fraction = ''] = raw.split('.');
  return 'R$ ' + whole.replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ',' + fraction.padEnd(2, '0');
}
async function request(url, options = {}) {
  const controller = new AbortController();
  const timer = setTimeout(() => controller.abort(), 10000);
  try { return await fetch(url, { ...options, signal: controller.signal, cache: 'no-store' }); }
  finally { clearTimeout(timer); }
}
$('discount-form').addEventListener('submit', async event => {
  event.preventDefault();
  if ($('simulate').disabled) return;
  const amount = Number($('amount').value), discount = Number($('discount').value);
  const customerType = $('customer-type').value;
  const result = $('result');
  result.classList.remove('error');
  if (!Number.isFinite(amount) || amount <= 0 || amount > 1e9 || !Number.isFinite(discount) || discount < 0 || discount > 20) {
    result.textContent = 'Informe um valor positivo de até R$ 1 bilhão e um desconto entre 0% e 20%.';
    result.classList.add('error'); return;
  }
  $('simulate').disabled = true;
  result.textContent = 'Calculando…';
  try {
    const response = await request('/api/discount', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ amount, discount, customerType }) });
    const data = await response.json();
    if (!response.ok) throw new Error(data.error || 'Não foi possível calcular o desconto.');
    result.textContent = `Original: ${money(data.original)}\nDesconto: ${money(data.discount)}\nTotal: ${money(data.total)}`;
  } catch (error) {
    result.classList.add('error');
    result.textContent = error.name === 'AbortError' ? 'A consulta demorou demais. Tente novamente.' : error.message;
  } finally { $('simulate').disabled = false; }
});
function node(tag, content, className) { const item = document.createElement(tag); item.textContent = text(content); if (className) item.className = className; return item; }
function details(title, content, metadata) {
  const item = document.createElement('details'); item.append(node('summary', title));
  if (metadata) item.append(node('p', metadata, 'source-meta'));
  item.append(node('pre', content)); return item;
}
function replaceList(id, values, render, fallback) { const area = $(id); area.replaceChildren(); if (!values.length) area.append(node(id === 'tests' ? 'li' : 'p', fallback)); else values.forEach(value => area.append(render(value))); }
function render(report) {
  const demo = /offline|demo|replay/i.test(String(report.mode));
  $('mode').textContent = demo ? 'Demo preparada · sem modelo' : 'Modo informado: ' + text(report.mode);
  $('run-status').textContent = 'Status: ' + text(report.status);
  $('mode-explanation').textContent = demo ? 'As propostas e a falha inicial deste cenário são preparadas. Os testes são executados de verdade; isso não demonstra uma descoberta autônoma do modelo.' : 'Confira os eventos e as evidências: uma proposta do modelo não equivale a código aprovado.';
  $('model-calls').textContent = report.modelCalls ?? '—';
  $('corrections').textContent = report.corrections ?? '—';
  $('tokens').textContent = report.tokens == null ? 'Não informado' : text(report.tokens);
  replaceList('events', report.events || [], event => {
    const item = document.createElement('li');
    item.append(node('span', `${event.role} · ${event.stage}`, 'event-role'));
    if (Number.isFinite(event.elapsedMs)) item.append(node('span', `${event.elapsedMs} ms`, 'event-time'));
    item.append(node('p', event.message)); return item;
  }, 'Nenhum evento registrado.');
  replaceList('tests', report.tests || [], test => node('li', `${test.passed ? 'PASSOU' : 'FALHOU'} · ${test.name}${test.detail ? ' — ' + test.detail : ''}`, test.passed ? 'passed' : 'failed'), 'Nenhum teste registrado.');
  replaceList('sources', report.sources || [], source => details(`${source.id} · ${source.path}`, source.text, `SHA256: ${source.sha256 || 'não informado'} · Similaridade: ${source.score ?? 'não informada'}`), 'Nenhuma fonte registrada.');
  replaceList('proposals', report.proposals || [], proposal => details(proposal.role, proposal.content), 'Nenhuma proposta registrada.');
  $('run-content').hidden = false;
}
let loading = false, lastReport = '', hadReport = false;
async function refresh() {
  if (loading || document.hidden) return;
  loading = true; $('refresh').disabled = true;
  try {
    const response = await request('/api/run');
    if (response.status === 404) {
      $('run-message').textContent = 'Nenhuma execução disponível. Execute o runner para produzir o primeiro relatório.';
      $('run-content').hidden = true;
      replaceList('tests', [], null, 'Nenhum relatório carregado.');
      replaceList('sources', [], null, 'Nenhuma fonte carregada.');
      replaceList('proposals', [], null, 'Nenhuma proposta carregada.');
      lastReport = ''; hadReport = false; return;
    }
    if (!response.ok) throw new Error('Falha ao consultar o relatório.');
    const report = await response.json();
    const serialized = JSON.stringify(report);
    if (lastReport !== serialized) { render(report); lastReport = serialized; }
    hadReport = true;
    $('run-message').textContent = 'Relatório disponível. Os eventos abaixo foram registrados pelo runner.';
  } catch (error) {
    $('run-message').textContent = `Não foi possível atualizar. ${hadReport ? 'Exibindo o último relatório recebido. ' : ''}Use Atualizar para tentar novamente.`;
  } finally { loading = false; $('refresh').disabled = false; }
}
$('refresh').addEventListener('click', refresh);
document.addEventListener('visibilitychange', () => { if (!document.hidden) refresh(); });
setInterval(refresh, 2000);
refresh();
