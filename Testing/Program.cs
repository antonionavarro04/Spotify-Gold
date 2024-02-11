using System;
using System.Threading.Tasks;
using DAL; // Import the namespace where VideoFunctions is defined


string directoryPath = await VideoFunctions.DownloadVideo("C:/Users/Antonio Navarro/Downloads/", "dQw4w9WgXcQ");

// Use the returned directoryPath as needed
if (directoryPath != null) {
    Console.WriteLine($"Video downloaded successfully to: {directoryPath}");
} else {
    Console.WriteLine("Failed to download the video.");
}
