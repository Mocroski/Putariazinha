using System;
using System.Data.SqlClient;
using Dapper;

class Program
{
    static void Main()
    {
        string connectionString = $"Data Source=NONE;Initial Catalog=NONE;Persist Security Info=True;User Id=admin;Password=NONE";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var cu = connection.Query(@"SELECT
                                                PatrolIncidents.Id,
                                                PatrolIncidents.LocationVerificationRecordId,
                                                (
                                                    SELECT TOP(1) CheckPointVerification.Id
                                                    FROM AppCheckPointVerificationRecords AS CheckPointVerification
                                                    WHERE (CheckPointVerification.LocationVerificationRecordId = PatrolIncidents.LocationVerificationRecordId)
                                                        ) AS CheckPointVerificationRecordId,
                                                            PatrolIncidents.Description,
                                                            PatrolIncidents.CreationTime,
                                                            PatrolIncidents.CreatorId,
                                                            PatrolIncidents.LastModificationTime,
                                                            PatrolIncidents.TenantId,
                                                            PatrolIncidents.LastModifierId
                                                        FROM
                                                            AppPatrolIncidents AS PatrolIncidents
                                                            INNER JOIN (
                                                                SELECT
                                                                    LocationRecord.Id
                                                                FROM
                                                                    AppLocationVerificationRecords AS LocationRecord  
                                                            ) AS LocationVerification ON PatrolIncidents.LocationVerificationRecordId = LocationVerification.Id");
            foreach (var item in cu)
            {
                Console.WriteLine(item);
                if (item.CheckPointVerificationRecordId == null)
                {
                    var sexo = connection.QueryFirst(@$"SELECT TOP (1)
                                                        PatrolIncidents.Description AS CheckPointDescription,
                                                        CheckpointsJoin.Id AS CheckPointId,
                                                        LocationVerification.Id AS LocationVerificationRecordId,
                                                        PatrolIncidents.TenantId,
                                                        CheckpointsJoin.Longitude,
                                                        CheckpointsJoin.Latitude,
                                                        PatrolIncidents.CreatorId,
                                                        PatrolIncidents.CreationTime 
                                                            FROM
                                                                AppPatrolIncidents AS PatrolIncidents
                                                                    INNER JOIN (
                                                                        SELECT
                                                                            LocationRecord.Id,
                                                                            LocationRecord.LocationId 
                                                                        FROM
                                                                            AppLocationVerificationRecords AS LocationRecord
                                                                    ) AS LocationVerification ON PatrolIncidents.LocationVerificationRecordId = '{item.LocationVerificationRecordId}'
                                                                        INNER JOIN (
                                                                            SELECT TOP (1)
                                                                                CheckpointsInnerJoin.Id,
                                                                                CheckpointsInnerJoin.Latitude,
                                                                                CheckpointsInnerJoin.LocationId,
                                                                                CheckpointsInnerJoin.Longitude
                                                                                FROM
                                                                                    AppCheckPoints AS CheckpointsInnerJoin
                                                                            ) AS CheckpointsJoin ON LocationVerification.LocationId = CheckpointsJoin.LocationId
                                                    ");

                  

                        var newCuzinho = Guid.NewGuid();

                        var novoSexo = connection.Execute(
                            "INSERT INTO [AppCheckPointVerificationRecords] (Id, TenantId, QrCodeConfirmed, Longitude, Latitude, CheckPointId, CreationTime, CreatorId, CheckPointDescription, LocationVerificationRecordId) " +
                            "VALUES (@Id, @TenantId, @QrCodeConfirmed, @Longitude, @Latitude, @CheckPointId, @CreationTime, @CreatorId, @CheckPointDescription, @LocationVerificationRecordId)",
                            new
                            {
                                Id = newCuzinho,
                                TenantId = sexo.TenantId,
                                QrCodeConfirmed = false,
                                Longitude = sexo.Longitude,
                                Latitude = sexo.Latitude,
                                CheckPointId = sexo.CheckPointId,
                                CreationTime = sexo.CreationTime,
                                CreatorId = sexo.CreatorId,
                                CheckPointDescription = sexo.CheckPointDescription,
                                LocationVerificationRecordId = item.LocationVerificationRecordId
                            });


                        var newCuzinho2 = Guid.NewGuid();

                        var analzinho = connection.Execute(
                           "INSERT INTO [AppCheckPointIncidents] (Id, Description, CheckPointVerificationRecordId, TenantId, CreationTime, CreatorId) " +
                           "VALUES (@Id, @Description, @CheckPointVerificationRecordId, @TenantId, @CreationTime, @CreatorId)",
                           new
                           {
                               Id = newCuzinho2,
                               Description = item.Description,
                               CheckPointVerificationRecordId = newCuzinho,
                               TenantId = item.TenantId,
                               CreationTime = item.CreationTime,
                               CreatorId = item.CreatorId
                           });

                        Console.WriteLine(analzinho);
                    
                }
                else
                {
                    var newcuzinho3 = Guid.NewGuid();

                    var analzinho2 = connection.Execute(
                           "INSERT INTO [AppCheckPointIncidents] (Id, Description, CheckPointVerificationRecordId, TenantId, CreationTime, CreatorId) " +
                           "VALUES (@Id, @Description, @CheckPointVerificationRecordId, @TenantId, @CreationTime, @CreatorId)",
                           new
                           {
                               Id = newcuzinho3,
                               Description = item.Description,
                               CheckPointVerificationRecordId = item.CheckPointVerificationRecordId,
                               TenantId = item.TenantId,
                               CreationTime = item.CreationTime,
                               CreatorId = item.CreatorId
                           });

                    Console.WriteLine(analzinho2);
                }
            }
            }

            Console.WriteLine("QUERO DA OCU");
        }
    }

