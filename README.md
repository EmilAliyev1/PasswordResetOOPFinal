# Password Reset & Brute-Force Benchmarker

## Development Stages & Version History

### Phase 1: Foundation & Base Infrastructure
* **Initialized Project Structure:** Established the primary workspace, solution folders, and clean configurations (**Commit #1: initialize project structure** and **Commit #2: delete bin and obj folders**).
* **Core Password Generation:** Implemented the initial `PasswordGenerator` service alongside an entry-level UI TextBox and Button interface to verify that random baseline string creation worked reliably (**Commit #3: implement PasswordGenerator, add TextBox and Button to test the PasswordGenerator**).
* **Cryptographic Layer:** Created the static `PasswordHasher` class using SHA-256 serialization and salt configurations to securely evaluate matching pairs (**Commit #4: implement PasswordHasher**).
* **Validation Subsystem:** Created the `PasswordValidator` utility to compare plaintext character arrays against static encrypted hash payloads (**Commit #5: implement PasswordValidator**).

### Phase 2: Sequential Engine & Generation Algorithms
* **Combinations Generation:** Implemented the `BruteForceGenerator` to handle character mapping arrays and dynamic permutations, expanding the maximum generation bounds from 6 to 7 characters to increase runtime tracking depth (**Commit #6: implement BruteForceGenerator, increase the password's max length in the PasswordGenerator from 6 to 7**).
* **Abstract Cracking Interface:** Structured the `IBruteForceCracker` interface to lay down common behavior contracts, and created initial empty stubs for single and multi-threaded tracking components (**Commit #7: implement IBruteForceCracker, scaffold MultiThreadCracker and SingleThreadCracker stub, minor refactoring**).
* **Sequential Processing Execution:** Implemented the inner cracking loop for `SingleThreadCracker` to systematically step through string variants one at a time (**Commit #8: implement SingleThreadCracker's Crack method**).
* **Algorithmic Constraint Adjustment:** Adjusted the password configuration space back to its default baseline parameters by reducing the maximum character bounds from 7 back down to 6 (**Commit #9: minor change**).
* **UI Thread Isolation:** Refactored runtime execution by wrapping tasks in `Task.Run()`, ensuring the background crunching work never locks up or freezes the Windows UI thread (**Commit #10: make CrackAsync non-blocking with Task.Run**).

### Phase 3: Concurrency Engine & Parallel Optimization
* **Concurrency Loop Architecture:** Fully implemented the `MultiThreadCracker` component, upgrading processing from sequential execution to high-concurrency iteration utilizing .NET's `Parallel.ForEach` (**Commit #11: implement MultiThreadCracker's CrackAsync Method**).
* **State Management Cleanup:** Deleted the empty Models structural storage folder to keep the code footprint clean and direct (**Commit #12: delete Models folder**).

### Phase 4: System Integration & Diagnostic Benchmarking
* **Persistent Performance Tracking:** Implemented the static `PerformanceLogger` class to seamlessly capture metric histories and serialize benchmark text files to disk (**Commit #13: implement PerformanceLogger**).
* **Full Application Wiring:** Fully integrated the complete Model-View-ViewModel design pattern. This tier tied the primary user view commands to asynchronous endpoints, passed functional `CancellationToken` objects down to parallel loops for immediate cancellation, instantiated the `BenchmarkResult` immutable storage record, and piped complete data over to the logger (**Commit #14: fully implement ui, pass cancellation token to the MultiThreadCracker and SingleThreadCracker, add BenchmarkResult, pass BenchmarkResult to LogPerformance**).
* **Visual Metric Adjustments:** Patched final UI string formulas and display layouts to make sure execution times and speedup comparisons render accurately on screen (**Commit #15: update displayed ElapsedTime text calculation**).

---

## Key Technical Features Demonstrated

1. **Cooperative Cancellation:** Uses a unified `CancellationTokenSource` to gracefully stop all active background threads mid-execution when a user interrupts the process via the UI.
2. **Thread Safety:** Implements atomic `Interlocked` increments and selection swaps (`Interlocked.Increment`, `Interlocked.CompareExchange`) within parallel sections to avoid standard memory corruption and data race conditions.
3. **Hardware-Optimized Parallelism:** Uses `Parallel.ForEach` configured with a dynamic `MaxDegreeOfParallelism` boundary that utilizes optimal system core allocation without overwhelming host systems.