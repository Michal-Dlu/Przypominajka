# A simple reminder app built with .NET MAUI.

Currently developed and tested mainly for Android.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Platform Support](#platform-support)
- [How to run](#how-to-run)
- [How to use the app](#how-to-use-the-app)

## Features
Add reminders
Local notifications
SQLite local storage
MVVM architecture (CommunityToolkit.Mvvm)
Cross-platform project structure (.NET MAUI)

## Tech Stack
.NET 9 / .NET MAUI
C#
SQLite (sqlite-net-pcl)
MVVM Toolkit
Plugin.LocalNotification

## Platform support

Currently working:

✔️ Android (tested)

Not fully tested:

🖥️ Windows (build included, not tested)
🍎 iOS (requires macOS)
🍏 MacCatalyst (requires macOS)

📦 Requirements

To run this project you need:

Visual Studio 2022 / 2025
.NET MAUI workload installed
Android SDK (installed via Visual Studio)
Android phone or emulator

## How to run
1. Clone repository
git clone https://github.com/Michal-Dlu/Przypominajka.git  
cd przypominajka

3. Open solution

Open:

Przypominajka.sln

in Visual Studio

3. Select target device

Choose one of:

Android Emulator 📱
Physical Android device 📱 (USB debugging enabled)

4. Run application

Press:

F5 (Start Debugging)

📱 Android setup (important)

If using a real phone:

Enable Developer Options
Enable USB Debugging
Accept RSA prompt on device

⚠️ Known issues
First deployment may fail if old version is installed → uninstall app from device and try again
ADB must detect device (adb devices should list it)
Some builds may require clean rebuild after ABI/config changes

📁 Project structure
/Platforms
/Views
/ViewModels
/Models
/Services
/Resources

🧠 Architecture

This project uses MVVM pattern:

Views → UI (XAML)
ViewModels → logic
Models → data
Services → database + notifications

📄 License

MIT License

## How to use the app

➕ Adding a new reminder

<img width="300" height="600" alt="Screenshot_20260622-162253" src="https://github.com/user-attachments/assets/6049ea78-6e82-4671-83ff-61860da6f567" />  
On the start screen, click “Przejdź do drugiej strony”.  

<img width="576" height="1283" alt="Screenshot_20260622-162303" src="https://github.com/user-attachments/assets/e9610a9c-24ea-41cb-8026-3c4439f20d1a" />  
Enter the name of the medication you want to save in the text field: "Wprowadź Nazwę i Dawkę Leku.  

<img width="576" height="1283" alt="Screenshot_20260622-162338" src="https://github.com/user-attachments/assets/159784b6-1683-4c67-b6b9-ecd76f3e7233" />  
Use the + or − buttons to set the time for the first dose.  

<img width="576" height="1283" alt="Screenshot_20260622-162353" src="https://github.com/user-attachments/assets/099b653b-c637-478f-923d-d39b46b7dcf4" />  
Select the dosage from the list by clicking “Wybierz dawkowanie”.  

<img width="576" height="1283" alt="Screenshot_20260622-162404" src="https://github.com/user-attachments/assets/8636879c-aa04-4f86-b49c-fe76ddea0cc2" />  
Finally, click “Zatwierdź” to save the medication.




