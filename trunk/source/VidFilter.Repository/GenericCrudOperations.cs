using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using VidFilter.Model;

namespace VidFilter.Repository
{
    class GenericCrudOperations
    {
        private IDocumentStore DocumentStore;

        #region Generic CRUD Operations

        public IEnumerable<T> Query<T>(params string[] Ids)
        {
            T[] records;
            using (var session = DocumentStore.OpenSession())
            {
                records = session.Load<T>(Ids);
            }
            return records;

        }

        public OperationStatus Insert<T>(params T[] records) where T : IMergeable
        {
            OperationStatus opStatus;
            int numRecordsUpdated = 0;

            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    foreach (T record in records)
                    {
                        T recordLookup = session.Query<T>().Where<T>(rec => rec.Id == record.Id).SingleOrDefault();
                        if (recordLookup != null)
                        {
                            record.MergeFrom(recordLookup);
                        }
                        else
                        {
                            numRecordsUpdated++;
                            session.Store(record);
                        }
                    }
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                opStatus = OperationStatus.GetOperationStatusFromException("Failure while adding Movie record", ex);
                return opStatus;
            }

            opStatus = new OperationStatus();
            opStatus.IsSuccess = true;
            opStatus.NumRecordsAffected = numRecordsUpdated;
            opStatus.Message = numRecordsUpdated + " " + typeof(T) + " records added";
            return opStatus;
        }

        public OperationStatus Delete<T>(params T[] records)
        {
            OperationStatus opStatus;
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    foreach (T record in records)
                    {
                        session.Delete<T>(record);
                    }
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                opStatus = OperationStatus.GetOperationStatusFromException("Failure deleting record.", ex);
                return opStatus;
            }

            opStatus = new OperationStatus();
            opStatus.IsSuccess = true;
            opStatus.Message = "Success deleting records";
            opStatus.NumRecordsAffected = records.Count();
            return opStatus;
        }

        public OperationStatus Update<T>(params T[] records) where T : IMergeable
        {
            OperationStatus opStatus;
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    foreach (T newRecord in records)
                    {
                        T oldRecord = session.Load<T>(newRecord.Id);
                        oldRecord.MergeFrom(newRecord);
                    }
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                opStatus = OperationStatus.GetOperationStatusFromException("Failure updating record", ex);
                return opStatus;
            }
            opStatus = new OperationStatus();
            opStatus.NumRecordsAffected = records.Count();
            opStatus.Message = "Success updating records";
            opStatus.IsSuccess = true;
            return opStatus;
        }
        #endregion

    }
}
