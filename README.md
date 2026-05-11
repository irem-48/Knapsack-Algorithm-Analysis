# Comparative Analysis of DP and GA for the Knapsack Problem

This project implements and compares two fundamental approaches to solve the **0/1 Knapsack Problem**: **Dynamic Programming (DP)** and **Genetic Algorithm (GA)**. The study focuses on analyzing the trade-off between execution time and solution accuracy across various dataset scales.

## 🚀 Key Features
- **Exact Solution:** Implementation of a bottom-up Dynamic Programming approach with $O(N \cdot W)$ complexity.
- **Heuristic Search:** Implementation of a Genetic Algorithm featuring selection, crossover, and mutation.
- **Scalability Testing:** Performance analysis conducted with $N=100, 1000, 10000$ items.

## 📊 Performance Results
The experiments revealed that while DP is highly efficient for small datasets, GA provides a **97% time reduction** for larger datasets ($N=10000$), albeit with an accuracy gap of approximately 35%.

### Execution Time Comparison
![Execution Time](time_analysis.png)
*(Logarithmic scale visualization of performance bottlenecks)*

### Accuracy Gap Analysis
![Accuracy Gap](accuracy_analysis.png)
*(Visual representation of the heuristic trade-off)*

## 💻 System Specifications
All performance tests were executed on the following hardware:
- **Device:** Asus TUF Gaming F15
- **Language:** C# / .NET
- **IDE:** Visual Studio 2022

## 📜 Author
**İrem Nisa Sözen** - Software Engineering Student
