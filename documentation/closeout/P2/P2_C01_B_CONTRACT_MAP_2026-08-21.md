# P2-C01-B Contract Map

**Baseline:** `master@c3f982d3f2c2197267af1bdfe4f0ddcd4df04d60`

| Capability | W1 | W2 | W3 | Primary permission |
|---|---|---|---|---|
| Pricing/payment-plan interaction | W1-P2C01-002; W1-P2C01-007 | W2-P2C01-011 | SHP-009; SHP-010 | `waybill.payment.plan` |
| Record actual collection | W1-P2C01-008; W1-P2C01-023 | W2-P2C01-012 | SHP-011 | `waybill.collection.create` |
| Reverse accepted collection | W1-P2C01-008; W1-P2C01-023 | W2-P2C01-013 | SHP-011 | `waybill.collection.reverse` |
| Derived payment status / remaining | W1-P2C01-002; W1-P2C01-007; W1-P2C01-008 | W2-P2C01-012; W2-P2C01-013 | SHP-012 | `waybill.view` / collection permissions |

## Boundary

Only these contracts are authorized for runtime realization in B. All W2 actions starting with Release/Trip/Allocation/Manifest and all later W3 screens remain contract-only until subsequent phases.
