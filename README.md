# Remote Desktop Monitoring System (VB.NET)

This is a remote control and monitoring application developed using VB.NET. The system consists of a **Server Program** and a **Client Program** that communicate over a network.

It allows the server user to **monitor**, **control**, and **interact** with connected client machines in real-time.

---

## 🖥️ Key Features

### ✅ Server Program:
- 📷 **Live Screen View** of the client’s desktop
- 🔁 **Screen Sharing**: Share the server’s screen to the client
- 🔍 **Monitor Running Applications** on the client PC
- ❌ **Close Any Running Application** on the client side
- ▶️ **Remote Command Execution** (e.g., run `calc.exe`, `notepad.exe`)
- 🌐 **Multiple Client Support** (can manage more than one connected client)

### ✅ Client Program:
- Runs in the background and listens for commands from the server
- Responds with live screenshots and system data
- Executes commands like opening/closing apps on request

---

## 🛠️ Technologies Used

- **VB.NET** (Windows Forms)
- **TCP/IP Sockets** for client-server communication
- **Multithreading** for handling multiple clients
- **System.Diagnostics** and **Process Management** in .NET
- **Image capturing** for screen sharing functionality

---

## 🗂️ Project Structure

- `Server/` – The server-side application
- `Client/` – The client-side application

Each contains a `.sln` file and Forms code.

---

## 🚀 How to Run

1. Open both `Server` and `Client` projects in **Visual Studio** (separately or as one solution)
2. Compile both applications
3. Run the **Client.exe** on the target machine
4. Run the **Server.exe** and connect to the client by its IP address
5. Start monitoring or sending commands

---

## ⚠️ Disclaimer

This software was developed for **academic/demo purposes only**. It should not be used without permission from the client machine’s user.

---

## 📸 Screenshots
<img width="1919" height="1199" alt="image" src="https://github.com/user-attachments/assets/c2572707-6723-4b2c-b9f6-1f6b530a3cce" />
<img width="1919" height="1199" alt="image" src="https://github.com/user-attachments/assets/5c1f44f7-9859-4011-9b0c-c5f682ded9e3" />
<img width="1919" height="1199" alt="image" src="https://github.com/user-attachments/assets/2fb15d69-c8a1-4de4-89e5-c177cb71d0f0" />

