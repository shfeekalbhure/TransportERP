# TransportERP — P2-C01 W0-5 Shared Kernel Scope

**Date:** 2026-08-20 UTC+3  
**Phase:** `W0-5 — Shared Kernel`  
**Baseline:** `master@96551dd2f99650f8e58a2df184bbdb95e6b0ff7e`  
**Status:** `IMPLEMENTATION_IN_PROGRESS`

## 1. Purpose

W0-5 implements provider-neutral shared contracts required by P2-C01 before any Waybill physical schema, migration, runtime API, or production SHP screen is created.

The phase exists to prevent each later module from inventing separate definitions for money/FX, operational-party snapshots, attachment metadata, movement metadata, geographic address snapshots, or server-authoritative numbering.

## 2. In scope

- `MoneyAmount` and immutable `FxSnapshot` conversion contract.
- `OperationalPartySnapshot`, `WaybillPartySnapshot`, and party role contract without automatic accounting-account creation.
- provider-neutral `AttachmentDescriptor` metadata with storage reference and content hash.
- append-only `MovementEnvelope` metadata with company/branch scope, correlation, retry identity, and reversal reference.
- `GeoAddressSnapshot` over the recovered governed Geo hierarchy.
- hardened `NumberReservation` states and `INumberReservationService` server-authoritative boundary.
- automated contract tests and a W0-5 phase validator.

## 3. Explicit exclusions

W0-5 MUST NOT create or modify:

- PostgreSQL tables or EF migrations;
- `P1Entities` or closed P1 lifecycle/schema;
- Waybill/WaybillItem/Trip/Manifest physical entities;
- runtime Waybill API endpoints;
- production SHP UI screens;
- accounting journal behavior;
- GPS, ticketing, maintenance, customs-full-domain, or last-mile runtime modules.

## 4. Authority rules

1. P1 remains closed and inherited.
2. W0-3 contract package is the functional authority for P2-C01.
3. Shared types must be provider-neutral and must not smuggle persistence decisions into Contracts.
4. Official numbering remains server-authoritative; client code cannot allocate final numbers.
5. Money always carries explicit currency identity; FX conversion uses an immutable historical snapshot.
6. Operational parties remain distinct from accounting accounts.
7. Attachment binary storage is external to the shared metadata contract.
8. Movement corrections are represented by new reversal/correction events; accepted event identity is never rewritten.

## 5. Closure conditions

W0-5 closes only when all conditions are true:

- shared contracts compile;
- existing P1/Foundation tests still pass;
- W0-5 shared-kernel tests pass;
- phase validator confirms no forbidden Infrastructure/Api/Desktop/P1 changes;
- no migration or physical schema file is introduced;
- review assignment exists before the PR is opened;
- final CI is green on the exact reviewed head;
- independent review returns explicit `PASS`;
- PR is merged to `master`.

Until then `P2-C01-A` MUST NOT start.
