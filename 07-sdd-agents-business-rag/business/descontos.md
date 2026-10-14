# Discount rules — B1

The original order amount must be finite and greater than zero.
The permitted discount percentage is from 0 to 20, inclusive.
Negative discounts and discounts above 20 must be rejected with a readable message.
The final amount is the original amount multiplied by (1 - discount / 100).
Currency amounts are rounded to two decimal places using HALF_UP.
Example: 100 with a 10 percent discount results in 90.00.
These rules belong to a fictional store used in this lab.
