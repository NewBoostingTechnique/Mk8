# TODO: Seed database and test everything... finish migrations.

# Mario Kart 8 Players' Page

TODO Find and Detail:
- Service.Detail throws if entity is not found.
- Service.Find returns null if entity is not found.
- Store.Detail return null if entity is not found.
- Store.Find doesn't exist.
- Table.Detail returns all matching rows (0, 1, or more).
--> Should we have 'Table.Find'?

Store interface doesn't/shouldn't care how databases work.
I.e.
- Store.Get should get.
- Store.Find should find.

Or we could say that 'Get' is by id, 'Find' is not/

## Naming Conventions

The same verb is used across all layers of an operation. These are the common verbs:

- **Create** - A new entity will be created.
- **Delete** - An existing entity will be deleted.
- **Detail** - A detail of a single existing entity will be obtained.
- **Exists** - An entity will be checked for existence, typically by key.
- **Find**
- **Identify**
- **Index**
- **Update**

### Presentation Layer

#### Components

- Player.Create.jsx

#### Functions

- Player_Create

### Web Layer

- PlayerApi
  
### Application Layer

#### Services

- IPlayerService.Create

#### Stores

- IPlayerStore.Create

### Data Layer

#### Scripts

- Player.Create.sql
- Player.Table.sql

#### Procedures

- Player.Create

## React

### Import Ordering

```
// 1. External Libraries
import React from 'react';
import { useState } from 'react';
import _ from 'lodash';

// 2. Absolute Imports (if using)
import { apiRequest } from 'utils/api';
import { formatDate } from 'helpers/date';

// 3. Parent Directory Imports
import UserCard from '../components/UserCard';

// 4. Same Directory Imports
import { useAuth } from './hooks/useAuth';

// 5. Side-Effect Imports
import './styles.css';
import 'regenerator-runtime/runtime';
```
