using System;
using System.Data.SqlClient;
using Dapper;

class Program
{
    static void Main()
    {
        string connectionString = $"Data Source=SEXO;Initial Catalog=SEXO;Persist Security Info=True;User Id=SEXO;Password=SEXO";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var cu = connection.Query("SELECT [a0].[Id], (\r\n    SELECT TOP(1) [a].[Id]\r\n    FROM [AppCheckPointVerificationRecords] AS [a]\r\n    WHERE ([a].[LocationVerificationRecordId] = [a0].[LocationVerificationRecordId])) AS [CheckPointVerificationRecordId], [a0].[Description], [a0].[CreationTime], [a0].[CreatorId], [a0].[LastModificationTime], [a0].[TenantId], [a0].[LastModifierId]\r\nFROM [AppPatrolIncidents] AS [a0]\r\nINNER JOIN (\r\n    SELECT [a1].[Id]\r\n    FROM [AppLocationVerificationRecords] AS [a1]  \r\n)AS [t] ON [a0].[LocationVerificationRecordId] = [t].[Id]");
            foreach (var item in cu)
            {
                Console.WriteLine(item);
                if (item.CheckPointVerificationRecordId == null)
                {
                    var sexo = connection.Query("SELECT TOP 1 [a].[Description] AS [CheckPointDescription], [t0].[Id] AS [CheckPointId], [t].[Id] \r\nAS [LocationVerificationRecordId], [a].[TenantId], [t0].[Longitude], [t0].[Latitude], [a].[CreatorId], [a].[CreationTime]\r\nFROM [AppPatrolIncidents] AS [a]\r\nINNER JOIN (\r\n    SELECT [a0].[Id], [a0].[LocationId]\r\n    FROM [AppLocationVerificationRecords] AS [a0]\r\n) AS [t] ON [a].[LocationVerificationRecordId] = [t].[Id]\r\nINNER JOIN (\r\n    SELECT TOP (1) [a1].[Id], [a1].[Latitude], [a1].[LocationId], [a1].[Longitude]\r\n    FROM [AppCheckPoints] AS [a1]\r\n) AS [t0] ON [t].[LocationId] = [t0].[LocationId]\r\nWHERE ([t].[Id] = [a].[LocationVerificationRecordId])");

                    foreach (var cum in sexo)
                    {
                        Console.WriteLine(cum);

                        var newCuzinho = Guid.NewGuid();

                        var novoSexo = connection.Execute(
                            "INSERT INTO [AppCheckPointVerificationRecords] (Id, TenantId, QrCodeConfirmed, Longitude, Latitude, CheckPointId, CreationTime, CreatorId, CheckPointDescription, LocationVerificationRecordId) " +
                            "VALUES (@Id, @TenantId, @QrCodeConfirmed, @Longitude, @Latitude, @CheckPointId, @CreationTime, @CreatorId, @CheckPointDescription, @LocationVerificationRecordId)",
                            new
                            {
                                Id = newCuzinho,
                                TenantId = cum.TenantId,
                                QrCodeConfirmed = false,
                                Longitude = cum.Longitude,
                                Latitude = cum.Latitude,
                                CheckPointId = cum.CheckPointId,
                                CreationTime = cum.CreationTime,
                                CreatorId = cum.CreatorId,
                                CheckPointDescription = cum.CheckPointDescription,
                                LocationVerificationRecordId = cum.LocationVerificationRecordId
                            });

                        var beforeSexo = connection.QueryFirstOrDefault($"SELECT * FROM AppCheckPointVerificationRecords WHERE ID = '{newCuzinho}'");

                        Console.WriteLine("BEFORESEXSO:");
                        Console.WriteLine(beforeSexo);

                        //if (connection.QueryFirstOrDefault($"SELECT * FROM AppCheckPointIncidents WHERE ID = '{item.Id}'") != null)
                        //{
                        //    continue;
                        //}

                        var analzinho = connection.Execute(
                           "INSERT INTO [AppCheckPointIncidents] (Id, Description, CheckPointVerificationRecordId, TenantId, CreationTime, CreatorId) " +
                           "VALUES (@Id, @Description, @CheckPointVerificationRecordId, @TenantId, @CreationTime, @CreatorId)",
                           new
                           {
                               Id = item.Id,
                               Description = item.Description,
                               CheckPointVerificationRecordId = beforeSexo.Id,
                               TenantId = item.TenantId,
                               CreationTime = item.CreationTime,
                               CreatorId = item.CreatorId
                           });

                        Console.WriteLine(analzinho);
                    }
                }

                //if (connection.QueryFirstOrDefault($"SELECT * FROM AppCheckPointIncidents WHERE ID = '{item.Id}'") != null)
                //{
                //    continue;
                //}


                else
                {
                    var analzinho = connection.Execute(
                           "INSERT INTO [AppCheckPointIncidents] (Id, Description, CheckPointVerificationRecordId, TenantId, CreationTime, CreatorId) " +
                           "VALUES (@Id, @Description, @CheckPointVerificationRecordId, @TenantId, @CreationTime, @CreatorId)",
                           new
                           {
                               Id = item.Id,
                               Description = item.Description,
                               CheckPointVerificationRecordId = item.CheckPointVerificationRecordId,
                               TenantId = item.TenantId,
                               CreationTime = item.CreationTime,
                               CreatorId = item.CreatorId
                           });

                    Console.WriteLine(analzinho);
                }
            }
            }

            Console.WriteLine("QUERO DA OCU");
        }
    }

