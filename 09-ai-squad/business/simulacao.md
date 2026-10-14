# Simulation calculation and validation

The purchase amount must be a positive decimal number and must not exceed the technical limit of R$1 billion. The percentage must be numeric, nonnegative, and within the profile limit. Reject numeric fields sent as text, missing values, and unknown profiles.

Use decimal arithmetic. Round currency to two decimal places with MidpointRounding.AwayFromZero. The total is the purchase amount multiplied by (1 - percentage / 100), rounded at the end. The response reports the applied percentage, not a discount amount in currency. Presentation must not lose cents when converting currency responses.

There are no payments, persistence, authentication, or integrations with real customers. Profile selection exists only to exercise the rule.
