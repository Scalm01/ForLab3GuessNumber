# Architecture Overview - VSC Extension Backend

This document provides a comprehensive architecture overview of how the browser/VSC extension integrates with the server backend for activity tracking and session management.

---

## System Architecture Diagram

```mermaid
graph TB
    subgraph EXT["Browser/VSC Extension"]
        E1["Activity Monitor"]
        E2["Session Manager"]
        E3["Network Handler"]
    end
    
    subgraph API["Express Routes / Controllers"]
        RS["session.js<br/>POST /start<br/>POST /confirm<br/>POST /end"]
        RI["ingest.js<br/>POST /ingest"]
    end
    
    subgraph LOGIC["Services"]
        SM["sessionManager.js<br/>startSession()<br/>touchSessionActivity()<br/>confirmAwake()<br/>endActiveSession()"]
        AD["adapter.js<br/>normalizePackets()"]
    end
    
    subgraph DB["MongoDB Collections"]
        AS["active_sessions<br/>userId, taskId, sessionId<br/>status, createdAt<br/>lastActivityAt"]
        S["sessions<br/>userId, taskId, sessionId<br/>reason, durationSeconds"]
        E["events<br/>userId, taskId, sessionId<br/>type, timestamp, eventContent"]
    end
    
    %% Extension to Routes
    EXT -->|"1. POST /start<br/>{userId, taskId}"| RS
    EXT -->|"3. POST /confirm<br/>{userId, taskId, sessionId}"| RS
    EXT -->|"4. POST /end<br/>{userId, taskId, sessionId, reason}"| RS
    EXT -->|"2. Periodic POST /ingest<br/>batch packets[]"| RI
    
    %% Routes to Logic
    RS -->|"Call"| SM
    RI -->|"Call"| AD
    RI -->|"Update activity"| SM
    
    %% Logic to DB
    SM -->|"CRUD"| AS
    SM -->|"Write ended sessions"| S
    RI -->|"Insert normalized events"| E
    AD -->|"Returns normalized packets"| RI
    
    classDef extension fill:#fff3cd,stroke:#ffc107,stroke-width:2px
    classDef route fill:#d1ecf1,stroke:#17a2b8,stroke-width:2px
    classDef logic fill:#d4edda,stroke:#28a745,stroke-width:2px
    classDef database fill:#f8d7da,stroke:#dc3545,stroke-width:2px
    
    class EXT extension
    class RS,RI route
    class SM,AD logic
    class AS,S,E database
```

---

## Detailed Flow Diagram

```mermaid
flowchart TD
    A["🔌 Browser/VSC Extension"] 
    
    %% ===== 1. START SESSION =====
    A -->|"1️⃣ POST /start<br/>{userId, taskId}"| B["📍 server/routes/session.js"]
    B -->|"Call"| C["🔧 sessionManager.startSession"]
    C -->|"Check/Create"| D["💾 active_sessions"]
    D -->|"EXISTS: Refresh lastActivityAt"| E["↩️ Return existing sessionId"]
    D -->|"NEW: Create sessionId<br/>session-Xyyyz"| F["✅ Insert new active session"]
    E --> G["📤 Return {sessionId}"]
    F --> G
    G --> A
    
    %% ===== 2. INGEST ACTIVITY =====
    A -->|"2️⃣ Periodic POST /ingest<br/>batch packets[]"| H["📍 server/routes/ingest.js"]
    H -->|"Call"| I["🔧 adapter.normalizePackets"]
    I -->|"Transform each packet"| J["Normalize to<br/>{userId, taskId, sessionId,<br/>type, t, eventContent}"]
    J -->|"Validate"| K{"sessionId present<br/>in all events?"}
    K -->|"❌ NO"| L["⚠️ 400 Bad Request"]
    K -->|"✅ YES"| M["🔍 Derive missing userId<br/>from active_sessions OR<br/>historical sessions"]
    M --> N["🔧 For each unique<br/>{userId, taskId, sessionId}"]
    N -->|"Call"| O["sessionManager<br/>.touchSessionActivity"]
    O -->|"Update"| P["💾 active_sessions<br/>lastActivityAt = now"]
    P --> Q["➕ Insert all normalized<br/>packets into events"]
    Q -->|"Write"| R["💾 events collection"]
    R --> S["📤 Return {inserted: count}"]
    S --> A
    
    %% ===== 3. KEEP-ALIVE =====
    A -->|"3️⃣ POST /confirm<br/>{userId, taskId, sessionId}"| T["📍 server/routes/session.js"]
    T -->|"Call"| U["🔧 sessionManager<br/>.confirmAwake"]
    U -->|"Update"| V["💾 active_sessions<br/>status=active<br/>awaitingConfirmation=false<br/>lastActivityAt=now"]
    V --> W["📤 Return {ok: true}"]
    W --> A
    
    %% ===== 4. END SESSION =====
    A -->|"4️⃣ POST /end<br/>{userId, taskId, sessionId, reason}"| X["📍 server/routes/session.js"]
    X -->|"Call"| Y["🔧 sessionManager<br/>.endActiveSession"]
    Y -->|"Calculate"| Z["⏱️ durationSeconds =<br/>floor(now - createdAt / 1000)"]
    Z -->|"Write"| AA["💾 sessions collection<br/>{userId, taskId, sessionId,<br/>reason, durationSeconds}"]
    AA -->|"Delete"| AB["💾 active_sessions<br/>Remove active session"]
    AB --> AC["📤 Return {ok: true,<br/>ended: true|false,<br/>durationSeconds}"]
    AC --> A
    
    style A fill:#fff3cd,stroke:#ffc107,stroke-width:3px
    style B,H,T,X fill:#d1ecf1,stroke:#17a2b8,stroke-width:2px
    style C,I,U,Y,O fill:#d4edda,stroke:#28a745,stroke-width:2px
    style D,P,R,V,AA,AB fill:#f8d7da,stroke:#dc3545,stroke-width:2px
    style E,F,G,L,M,N,Q,S,W,Z,AC fill:#e2e3e5,stroke:#6c757d,stroke-width:1px
```

---

## Key Components

### 1. **Browser/VSC Extension** 
- **Activity Monitor**: Tracks user interactions and generates activity packets
- **Session Manager**: Manages session lifecycle (start, keep-alive, end)
- **Network Handler**: Communicates with backend via HTTP POST requests

### 2. **Express Routes**

#### `session.js`
- **POST /start**: Initiates a new session or refreshes an existing one
- **POST /confirm**: Confirms extension is still active (keep-alive ping)
- **POST /end**: Terminates an active session

#### `ingest.js`
- **POST /ingest**: Receives batch activity packets for ingestion

### 3. **Business Logic Services**

#### `sessionManager.js`
| Method | Purpose |
|--------|---------|
| `startSession()` | Creates or refreshes active session in MongoDB |
| `touchSessionActivity()` | Updates lastActivityAt timestamp |
| `confirmAwake()` | Confirms session is active and updates status |
| `endActiveSession()` | Calculates duration and archives session |

#### `adapter.js`
| Method | Purpose |
|--------|---------|
| `normalizePackets()` | Transforms raw extension packets into standardized event format |

### 4. **MongoDB Collections**

#### `active_sessions`
```json
{
  "userId": "string",
  "taskId": "string",
  "sessionId": "string (session-Xyyyz)",
  "status": "active|inactive",
  "createdAt": "timestamp",
  "lastActivityAt": "timestamp",
  "awaitingConfirmation": "boolean",
  "pingStartedAt": "timestamp (optional)"
}
```

#### `sessions`
```json
{
  "userId": "string",
  "taskId": "string",
  "sessionId": "string",
  "reason": "string (default: 'ended')",
  "durationSeconds": "number",
  "createdAt": "timestamp",
  "endedAt": "timestamp"
}
```

#### `events`
```json
{
  "userId": "string",
  "taskId": "string",
  "sessionId": "string",
  "type": "string",
  "timestamp": "number (t field)",
  "eventContent": "object",
  "metrics": "object (optional)"
}
```

---

## Activity Flow Summary

| Step | Action | Input | Output | DB Operation |
|------|--------|-------|--------|--------------|
| **1. Start** | Create/refresh session | `{userId, taskId}` | `{sessionId}` | Insert/Update `active_sessions` |
| **2. Ingest** | Process activity packets | `batch packets[]` | `{inserted: count}` | Insert `events`; Update `active_sessions.lastActivityAt` |
| **3. Confirm** | Keep-alive ping | `{userId, taskId, sessionId}` | `{ok: true}` | Update `active_sessions.lastActivityAt` |
| **4. End** | Close session | `{userId, taskId, sessionId, reason}` | `{durationSeconds}` | Insert `sessions`; Delete from `active_sessions` |

---

## Data Flow Sequence

```mermaid
sequenceDiagram
    participant Ext as Extension
    participant API as API Routes
    participant Svc as Services
    participant DB as MongoDB
    
    Ext->>API: 1. POST /start {userId, taskId}
    API->>Svc: startSession()
    Svc->>DB: Check/Insert active_sessions
    DB-->>Svc: Return sessionId
    Svc-->>API: sessionId
    API-->>Ext: {sessionId}
    
    Ext->>API: 2. Periodic POST /ingest [packets]
    API->>Svc: normalizePackets()
    Svc->>Svc: Validate & derive userId
    Svc->>Svc: touchSessionActivity()
    Svc->>DB: Update active_sessions.lastActivityAt
    API->>DB: Insert normalized events
    DB-->>API: Inserted count
    API-->>Ext: {inserted: count}
    
    Ext->>API: 3. POST /confirm {sessionId}
    API->>Svc: confirmAwake()
    Svc->>DB: Update active_sessions (status, lastActivityAt)
    DB-->>Svc: OK
    Svc-->>API: {ok: true}
    API-->>Ext: {ok: true}
    
    Ext->>API: 4. POST /end {sessionId, reason}
    API->>Svc: endActiveSession()
    Svc->>Svc: Calculate durationSeconds
    Svc->>DB: Insert sessions record
    Svc->>DB: Delete from active_sessions
    DB-->>Svc: OK
    Svc-->>API: {durationSeconds}
    API-->>Ext: {durationSeconds}
```

---

## Error Handling

| Scenario | Status Code | Response |
|----------|------------|----------|
| Missing `sessionId` in ingest packets | `400` | Error message: sessionId required |
| Duplicate active session | `200` | Return existing `sessionId` |
| Unknown userId in ingest | `200` | Lookup from `active_sessions` or historical `sessions` |
| Session not found on end | `200` | `ended: false` |

---

## Implementation Notes

- **Session ID Format**: `session-Xyyyz` where `X` and `y`, `z` are alphanumeric identifiers
- **Activity Heartbeat**: Updated on every ingest batch for continuous session tracking
- **AFK Handling**: Extension confirms it's still active via `/confirm` endpoint
- **Duration Calculation**: Server-side, based on timestamps stored in MongoDB
- **Atomic Operations**: Session start checks for existing sessions to prevent duplicates
