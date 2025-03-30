namespace InventoryManagement.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Windows;
    using InventoryManagement.Models;

    public static class ImportFromJson
    {
        public delegate void AddToDatabase<T>(T obj);
        public delegate void SaveChanges();

        public static void ImportObjectListWithTransaction<T>(List<T> objects, AddToDatabase<T> addToDatabase, SaveChanges saveChanges)
        {
            using (var transaction = InventoryManagementContext.INSTANCE.Database.BeginTransaction())
            {
                try
                {
                    foreach (var obj in objects)
                    {
                        addToDatabase(obj); // Add each object to the database
                    }

                    saveChanges(); // Save the changes
                    transaction.Commit(); // Commit the transaction
                    MessageBox.Show("Nhập dữ liệu thành công");
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback transaction on error
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static void ImportJsonData<T>(string filePath, AddToDatabase<T> addToDatabase, SaveChanges saveChanges)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<List<T>>(json);

                if (data != null)
                {
                    ImportObjectListWithTransaction(data, addToDatabase, saveChanges);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi phân tích JSON: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
