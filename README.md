```mermaid
flowchart TD
    A([Code Commit/Push]):::start --> B[Set up job]:::setup
    B --> C[Checkout<br/>Code]:::setup
    C --> D[Setup<br/>MSBuild]:::setup
    D --> E[Setup<br/>NuGet]:::setup
    E --> F[Restore<br/>NuGet Packages]:::setup
    F --> G[Build<br/>Main Project]:::build
    G --> H[Run Test]:::test
    H --> I[Post<br/>Checkout Code]:::cleanup
    I --> J([Complete Job]):::finish

    %% Färgteman + större text + svart färg
    classDef start fill:#bbdefb,stroke:#0d47a1,stroke-width:2px,font-size:14px,color:#000000;
    classDef setup fill:#c8e6c9,stroke:#2e7d32,stroke-width:2px,font-size:14px,color:#000000;
    classDef build fill:#fff9c4,stroke:#fbc02d,stroke-width:2px,font-size:14px,color:#000000;
    classDef test fill:#ffe0b2,stroke:#ef6c00,stroke-width:2px,font-size:14px,color:#000000;
    classDef cleanup fill:#eeeeee,stroke:#424242,stroke-width:2px,font-size:14px,color:#000000;
    classDef finish fill:#ffcdd2,stroke:#b71c1c,stroke-width:2px,font-size:14px,color:#000000;
