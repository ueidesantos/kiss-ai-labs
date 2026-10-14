# Profile-based discount policy

Public educational document. It does not contain a real company's policies.

- Standard profile allows a discount from 0% to 10%, inclusive.
- VIP profile allows a discount from 0% to 20%, inclusive.
- An order above the profile limit must be rejected; do not silently lower the percentage.
- The profile must be provided as Standard or VIP. An unknown profile is invalid.
- This request changes the Standard limit to 10%; keeping 20% for everyone is incorrect.

Examples: Standard at 15% must fail. VIP at 15% must work. Standard at 10% and VIP at 20% are valid limits.
