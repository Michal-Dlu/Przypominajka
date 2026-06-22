# A simple reminder app built with .NET MAUI.

Currently developed and tested mainly for Android.

## Features
## Tech Stack
## Platform Support

## Features
Add reminders
Local notifications
SQLite local storage
MVVM architecture (CommunityToolkit.Mvvm)
Cross-platform project structure (.NET MAUI)

🛠️ Tech Stack
.NET 9 / .NET MAUI
C#
SQLite (sqlite-net-pcl)
MVVM Toolkit
Plugin.LocalNotification

📱 Platform support

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

▶️ How to run
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
