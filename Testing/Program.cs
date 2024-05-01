string currentPath = Path.Combine(Directory.GetCurrentDirectory(), "ui.jpg");
byte[] image = File.ReadAllBytes(currentPath);

Console.WriteLine(DAL.ImageHandler.SaveImage(image)); // 1, pls
