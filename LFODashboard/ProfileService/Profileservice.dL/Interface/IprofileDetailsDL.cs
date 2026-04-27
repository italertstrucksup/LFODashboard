using ProfileService_LFO.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProfileService_LFO.DAL.Interface
{
    public interface IprofileDetailsDL
    {

        Task<string> UpdateFleetOperator(UpdateFleetOperatorRequest request);

        Task<string> InsertFleetOperatorbyType(UpdateFleetOperatorRequest request);

        Task<string> InsertTruckDetails(TruckDetailsRequest request);

        Task<string> InsertFleetOperatorDocument(UpdateDocumentRequest request);

        Task<string> InsertPreferredLane(PreferredLaneRequest request);

        Task<string> InsertFleetOperatorKYC(KYCRequest request);
    }
}