# ElRahmani Tracker

A simple **C# (.NET)** console application that tracks the recent activity of Codeforces users. It was developed to monitor the weekly progress of our **ECPC team** by fetching accepted submissions directly from the Codeforces API and generating useful statistics.

---

## Features

- Fetches accepted submissions using the official Codeforces API.
- Counts only **unique solved problems**.
- Filters problems solved during the **last 7 days**.
- Displays:
  - Total solved problems.
  - Number of **800-rated** problems.
  - Number of problems with **rating greater than 800**.
- Shows a detailed list of solved problems for each team member.
- Interactive console menu.

---

## Team Members

The tracker currently monitors:

- **Mazen_Eltelbany**
- **El-Abiad**
- **ElAmino**

You can easily modify the usernames inside `Program.cs` to track your own team.

---

## Screenshot

```markdown
[Tracker Screenshot](assets/project.png)
```
---

## Technologies Used

- C#
- .NET
- HttpClient
- LINQ
- System.Text.Json
- Codeforces REST API

---

## Project Structure

```text
ElRahmani-Tracker/
│
├── ElRahmani-Tracker.sln
├── ElRahmaniTracker.csproj
├── Program.cs
├── README.md
├── .gitignore
├── bin/
└── obj/
```

---

## Requirements

- .NET SDK 8.0 or later
- Internet connection
- A valid Codeforces account

---

# Installation

## Windows

### Clone the repository

```bash
git clone https://github.com/<YOUR_USERNAME>/ElRahmani-Tracker.git
```

### Enter the project directory

```bash
cd ElRahmani-Tracker
```

### Restore packages

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

---

## Arch Linux

### Install the .NET SDK

```bash
sudo pacman -S dotnet-sdk
```

Verify the installation:

```bash
dotnet --version
```

### Clone the repository

```bash
git clone https://github.com/<YOUR_USERNAME>/ElRahmani-Tracker.git
```

### Enter the project directory

```bash
cd ElRahmani-Tracker
```

### Restore dependencies

```bash
dotnet restore
```

### Build the project

```bash
dotnet build
```

### Run the application

```bash
dotnet run
```

---

## How It Works

The application sends a request to the Codeforces API:

```
https://codeforces.com/api/user.status
```

For each team member it:

1. Downloads all submissions.
2. Keeps only accepted submissions.
3. Removes duplicate solved problems.
4. Stores the earliest accepted submission.
5. Filters problems solved during the last seven days.
6. Calculates weekly statistics.

---

## License

This project is licensed under the MIT License.

---

## Author

**Mazen Eltelbany**

Computer Science Student | Backend Developer | Competitive Programmer
