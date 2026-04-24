using System;
using System.Collections.Generic;
using System.Text;

namespace VendorService.Models
{
    public class RCDetailsAPIRequest
    {
        public string rc_number { get; set; }
        public string consent { get; set; } = "Y";
    }

    public class RcDetailsResponse
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string Messagehindi { get; set; }
        public long reference_id { get; set; }
        public RcData data { get; set; }
    }

    public class RcData
    {
        public string client_id { get; set; }
        public string rc_number { get; set; }
        public string registration_date { get; set; }
        public string owner_name { get; set; }
        public string father_name { get; set; }
        public string present_address { get; set; }
        public string permanent_address { get; set; }
        public string vehicle_category { get; set; }
        public string vehicle_chasi_number { get; set; }
        public string vehicle_engine_number { get; set; }
        public string maker_description { get; set; }
        public string maker_model { get; set; }
        public string body_type { get; set; }
        public string fuel_type { get; set; }
        public string color { get; set; }
        public string norms_type { get; set; }
        public string fit_up_to { get; set; }
        public string financer { get; set; }
        public string financed { get; set; }
        public string insurance_company { get; set; }
        public string insurance_policy_number { get; set; }
        public string insurance_upto { get; set; }
        public string manufacturing_date { get; set; }
        public string manufacturing_date_formatted { get; set; }
        public string registered_at { get; set; }
        public string latest_by { get; set; }
        public bool less_info { get; set; }
        public string tax_upto { get; set; }
        public string tax_paid_upto { get; set; }
        public string cubic_capacity { get; set; }
        public string vehicle_gross_weight { get; set; }
        public string no_cylinders { get; set; }
        public string seat_capacity { get; set; }
        public string sleeper_capacity { get; set; }
        public string standing_capacity { get; set; }
        public string wheelbase { get; set; }
        public string unladen_weight { get; set; }
        public string vehicle_category_description { get; set; }
        public string pucc_number { get; set; }
        public string pucc_upto { get; set; }
        public string permit_number { get; set; }
        public string permit_issue_date { get; set; }
        public string permit_valid_from { get; set; }
        public string permit_valid_upto { get; set; }
        public string permit_type { get; set; }
        public string national_permit_number { get; set; }
        public string national_permit_upto { get; set; }
        public string national_permit_issued_by { get; set; }
        public string non_use_status { get; set; }
        public string non_use_from { get; set; }
        public string non_use_to { get; set; }
        public string blacklist_status { get; set; }
        public string noc_details { get; set; }
        public string owner_number { get; set; }
        public string rc_status { get; set; }
        public bool masked_name { get; set; }
        public string challan_details { get; set; }
        public string variant { get; set; }
    }

}
