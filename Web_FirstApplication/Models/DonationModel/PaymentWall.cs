using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web_FirstApplication.Models.Donation
{
    public class PaymentWall
    {
        public abstract class Paymentwall_Base
        {

            /*
             * Paymentwall library version
             */
            const string LIB_VERSION = "1.0.0";

            /*
             * URLs for Paymentwall Pro
             */
            protected const string CHARGE_URL = "https://api.paymentwall.com/api/pro/v1/charge";
            protected const string SUBS_URL = "https://api.paymentwall.com/api/pro/v1/subscription";

            /*
             * API types
             */
            public const int API_VC = 1;
            public const int API_GOODS = 2;
            public const int API_CART = 3;

            /*
             * Controllers for APIs
             */
            protected const string CONTROLLER_PAYMENT_VIRTUAL_CURRENCY = "ps";
            protected const string CONTROLELR_PAYMENT_DIGITAL_GOODS = "subscription";
            protected const string CONTROLLER_PAYMENT_CART = "cart";

            /**
             * Signature versions
             */
            protected const int DEFAULT_SIGNATURE_VERSION = 3;
            protected const int SIGNATURE_VERSION_1 = 1;
            protected const int SIGNATURE_VERSION_2 = 2;
            protected const int SIGNATURE_VERSION_3 = 3;

            protected List<string> errors = new List<string>();

            /**
             * Paymentwall API type
             * @param int apiType
             */
            public static int apiType;

            /**
             * Paymentwall application key - can be found in your merchant area
             * @param string appKey
             */
            public static string appKey;

            /**
             * Paymentwall secret key - can be found in your merchant area
             * @param string secretKey
             */
            public static string secretKey;

            /**
             * Paymentwall Pro API Key
             * @param string proApiKey
             */
            public static string proApiKey;

            /*
             * @param int apiType API type
             */
            public static void setApiType(int apiType)
            {
                Paymentwall_Base.apiType = apiType;
            }

            public static int getApiType()
            {
                return Paymentwall_Base.apiType;
            }

            /*
             * @param string appKey application key of your application, can be found inside of your Paymentwall Merchant Account
             */
            public static void setAppKey(string appKey)
            {
                Paymentwall_Base.appKey = appKey;
            }

            public static string getAppKey()
            {
                return Paymentwall_Base.appKey;
            }

            /*
             *  @param string secretKey secret key of your application, can be found inside of your Paymentwall Merchant Account
             */
            public static void setSecretKey(string secretKey)
            {
                Paymentwall_Base.secretKey = secretKey;
            }

            public static string getSecretKey()
            {
                return Paymentwall_Base.secretKey;
            }

            /*
             * @param string proApiKey API key used for Pro authentication
             */
            public static void setProApiKey(string proApiKey)
            {
                Paymentwall_Base.proApiKey = proApiKey;
            }

            public static string getProApiKey()
            {
                return Paymentwall_Base.proApiKey;
            }

            /*
             * Fill the array with the errors found at execution
             * 
             * @param string err
             * @return int
             */
            protected int appendToErrors(string err)
            {
                this.errors.Add(err);
                return this.errors.Count;
            }

            /**
             * Return errors
             * 
             * @return List<string>
             */
            public List<string> getErrors()
            {
                return this.errors;
            }

            /*
             * Return error summary
             * 
             * @return string
             */
            public string getErrorSummary()
            {
                return string.Join("\n", this.getErrors());
            }
        }
        public class Paymentwall_Pingback : Paymentwall_Base
        {
            /**
             * Pingback types
             */
            const int PINGBACK_TYPE_REGULAR = 0;
            const int PINGBACK_TYPE_GOODWILL = 1;
            const int PINGBACK_TYPE_NEGATIVE = 2;

            const int PINGBACK_TYPE_RISK_UNDER_REVIEW = 200;
            const int PINGBACK_TYPE_RISK_REVIEWED_ACCEPTED = 201;
            const int PINGBACK_TYPE_RISK_REVIEWED_DECLINED = 202;

            const int PINGBACK_TYPE_RISK_AUTHORIZATION_VOIDED = 203;

            const int PINGBACK_TYPE_SUBSCRIPTION_CANCELLATION = 12;
            const int PINGBACK_TYPE_SUBSCRIPTION_EXPIRED = 13;
            const int PINGBACK_TYPE_SUBSCRIPTION_PAYMENT_FAILED = 14;


            /**
             * Pingback parameters
             */
            protected Dictionary<string, string> parameters = new Dictionary<string, string>();


            /**
             * IP address
             */
            protected string ipAddress;


            /**
             * @param Dictionary<string, string> parameters associative array of parameters received by pingback processing script, e.g. Request.QueryString()
             * @param string ipAddress IP address from where the pingback request orginates, e.g. '127.0.0.1'
             */
            public Paymentwall_Pingback(NameValueCollection parameters, string ipAddress)
            {
                foreach (string p in parameters.AllKeys)
                {
                    this.parameters.Add(p, parameters[p]);
                }
                this.ipAddress = ipAddress;
            }


            /**
             * Check whether pingback is valid
             * 
             * @param bool skipIpWhiteListCheck if IP whitelist check should be skipped, e.g. if you have a load-balancer changing the IP
             * @return bool
             */
            public bool validate(bool skipIpWhiteListCheck = false)
            {
                bool validated = false;

                if (this.isParametersValid())
                {
                    if (this.isIpAddressValid() || skipIpWhiteListCheck)
                    {
                        if (this.isSignatureValid())
                        {
                            validated = true;
                        }
                        else
                        {
                            this.appendToErrors("Wrong signature");
                        }
                    }
                    else
                    {
                        this.appendToErrors("IP address is not whitelisted");
                    }
                }
                else
                {
                    this.appendToErrors("Missing parameters");
                }

                return validated;
            }


            /**
             * @return bool
             */
            public bool isSignatureValid()
            {
                string signature = "";
                Dictionary<string, string> signatureParamsToSign = new Dictionary<string, string>();
                if (this.parameters.ContainsKey("sig"))
                {
                    signature = this.parameters["sig"];
                }
                else
                {
                    signature = null;
                }

                List<string> signatureParams = new List<string>();

                if (Paymentwall_Pingback.getApiType() == Paymentwall_Pingback.API_VC)
                {
                    signatureParams.AddRange(new string[] { "uid", "currency", "type", "ref" });
                }
                else if (Paymentwall_Pingback.getApiType() == Paymentwall_Pingback.API_GOODS)
                {
                    signatureParams.AddRange(new string[] { "uid", "goodsid", "slength", "speriod", "type", "ref" });
                }
                else
                { //API_CART
                    signatureParams.AddRange(new string[] { "uid", "goodsid", "type", "ref" });
                    this.parameters["sign_version"] = Paymentwall_Pingback.SIGNATURE_VERSION_2.ToString();
                }

                if (!this.parameters.ContainsKey("sign_version")) //Check if signature version 1            
                {
                    foreach (string field in signatureParams)
                    {
                        if (this.parameters[field] != null)
                            signatureParamsToSign.Add(field, this.parameters[field]);
                        else
                            signatureParamsToSign.Add(field, null);
                    }
                    this.parameters["sign_version"] = Paymentwall_Pingback.SIGNATURE_VERSION_1.ToString();
                }
                else
                {
                    signatureParamsToSign = this.parameters;
                }

                string signatureCalculated = this.calculateSignature(signatureParamsToSign, Paymentwall_Pingback.getSecretKey(), Convert.ToInt32(this.parameters["sign_version"]));

                return signatureCalculated == signature;
            }


            /**
             * @return bool
             */
            public bool isIpAddressValid()
            {
                List<string> ipWhitelist = new List<string>() {
                "174.36.92.186",
                "174.36.96.66",
                "174.36.92.187",
                "174.36.92.192",
                "174.37.14.28",
                "216.127.71.0",
                "216.127.71.1",
                "216.127.71.2",
                "216.127.71.3",
                "216.127.71.4",
                "216.127.71.5",
                "216.127.71.6",
                "216.127.71.7",
                "216.127.71.8",
                "216.127.71.9",
                "216.127.71.10",
                "216.127.71.11",
                "216.127.71.12",
                "216.127.71.13",
                "216.127.71.14",
                "216.127.71.15",
                "216.127.71.16",
                "216.127.71.17",
                "216.127.71.18",
                "216.127.71.19",
                "216.127.71.20",
                "216.127.71.21",
                "216.127.71.22",
                "216.127.71.23",
                "216.127.71.24",
                "216.127.71.25",
                "216.127.71.26",
                "216.127.71.27",
                "216.127.71.28",
                "216.127.71.29",
                "216.127.71.30",
                "216.127.71.31",
                "216.127.71.32",
                "216.127.71.33",
                "216.127.71.34",
                "216.127.71.35",
                "216.127.71.36",
                "216.127.71.37",
                "216.127.71.38",
                "216.127.71.39",
                "216.127.71.40",
                "216.127.71.41",
                "216.127.71.42",
                "216.127.71.43",
                "216.127.71.44",
                "216.127.71.45",
                "216.127.71.46",
                "216.127.71.47",
                "216.127.71.48",
                "216.127.71.49",
                "216.127.71.50",
                "216.127.71.51",
                "216.127.71.52",
                "216.127.71.53",
                "216.127.71.54",
                "216.127.71.55",
                "216.127.71.56",
                "216.127.71.57",
                "216.127.71.58",
                "216.127.71.59",
                "216.127.71.60",
                "216.127.71.61",
                "216.127.71.62",
                "216.127.71.63",
                "216.127.71.64",
                "216.127.71.65",
                "216.127.71.66",
                "216.127.71.67",
                "216.127.71.68",
                "216.127.71.69",
                "216.127.71.70",
                "216.127.71.71",
                "216.127.71.72",
                "216.127.71.73",
                "216.127.71.74",
                "216.127.71.75",
                "216.127.71.76",
                "216.127.71.77",
                "216.127.71.78",
                "216.127.71.79",
                "216.127.71.80",
                "216.127.71.81",
                "216.127.71.82",
                "216.127.71.83",
                "216.127.71.84",
                "216.127.71.85",
                "216.127.71.86",
                "216.127.71.87",
                "216.127.71.88",
                "216.127.71.89",
                "216.127.71.90",
                "216.127.71.91",
                "216.127.71.92",
                "216.127.71.93",
                "216.127.71.94",
                "216.127.71.95",
                "216.127.71.96",
                "216.127.71.97",
                "216.127.71.98",
                "216.127.71.99",
                "216.127.71.100",
                "216.127.71.101",
                "216.127.71.102",
                "216.127.71.103",
                "216.127.71.104",
                "216.127.71.105",
                "216.127.71.106",
                "216.127.71.107",
                "216.127.71.108",
                "216.127.71.109",
                "216.127.71.110",
                "216.127.71.111",
                "216.127.71.112",
                "216.127.71.113",
                "216.127.71.114",
                "216.127.71.115",
                "216.127.71.116",
                "216.127.71.117",
                "216.127.71.118",
                "216.127.71.119",
                "216.127.71.120",
                "216.127.71.121",
                "216.127.71.122",
                "216.127.71.123",
                "216.127.71.124",
                "216.127.71.125",
                "216.127.71.126",
                "216.127.71.127",
                "216.127.71.128",
                "216.127.71.129",
                "216.127.71.130",
                "216.127.71.131",
                "216.127.71.132",
                "216.127.71.133",
                "216.127.71.134",
                "216.127.71.135",
                "216.127.71.136",
                "216.127.71.137",
                "216.127.71.138",
                "216.127.71.139",
                "216.127.71.140",
                "216.127.71.141",
                "216.127.71.142",
                "216.127.71.143",
                "216.127.71.144",
                "216.127.71.145",
                "216.127.71.146",
                "216.127.71.147",
                "216.127.71.148",
                "216.127.71.149",
                "216.127.71.150",
                "216.127.71.151",
                "216.127.71.152",
                "216.127.71.153",
                "216.127.71.154",
                "216.127.71.155",
                "216.127.71.156",
                "216.127.71.157",
                "216.127.71.158",
                "216.127.71.159",
                "216.127.71.160",
                "216.127.71.161",
                "216.127.71.162",
                "216.127.71.163",
                "216.127.71.164",
                "216.127.71.165",
                "216.127.71.166",
                "216.127.71.167",
                "216.127.71.168",
                "216.127.71.169",
                "216.127.71.170",
                "216.127.71.171",
                "216.127.71.172",
                "216.127.71.173",
                "216.127.71.174",
                "216.127.71.175",
                "216.127.71.176",
                "216.127.71.177",
                "216.127.71.178",
                "216.127.71.179",
                "216.127.71.180",
                "216.127.71.181",
                "216.127.71.182",
                "216.127.71.183",
                "216.127.71.184",
                "216.127.71.185",
                "216.127.71.186",
                "216.127.71.187",
                "216.127.71.188",
                "216.127.71.189",
                "216.127.71.190",
                "216.127.71.191",
                "216.127.71.192",
                "216.127.71.193",
                "216.127.71.194",
                "216.127.71.195",
                "216.127.71.196",
                "216.127.71.197",
                "216.127.71.198",
                "216.127.71.199",
                "216.127.71.200",
                "216.127.71.201",
                "216.127.71.202",
                "216.127.71.203",
                "216.127.71.204",
                "216.127.71.205",
                "216.127.71.206",
                "216.127.71.207",
                "216.127.71.208",
                "216.127.71.209",
                "216.127.71.210",
                "216.127.71.211",
                "216.127.71.212",
                "216.127.71.213",
                "216.127.71.214",
                "216.127.71.215",
                "216.127.71.216",
                "216.127.71.217",
                "216.127.71.218",
                "216.127.71.219",
                "216.127.71.220",
                "216.127.71.221",
                "216.127.71.222",
                "216.127.71.223",
                "216.127.71.224",
                "216.127.71.225",
                "216.127.71.226",
                "216.127.71.227",
                "216.127.71.228",
                "216.127.71.229",
                "216.127.71.230",
                "216.127.71.231",
                "216.127.71.232",
                "216.127.71.233",
                "216.127.71.234",
                "216.127.71.235",
                "216.127.71.236",
                "216.127.71.237",
                "216.127.71.238",
                "216.127.71.239",
                "216.127.71.240",
                "216.127.71.241",
                "216.127.71.242",
                "216.127.71.243",
                "216.127.71.244",
                "216.127.71.245",
                "216.127.71.246",
                "216.127.71.247",
                "216.127.71.248",
                "216.127.71.249",
                "216.127.71.250",
                "216.127.71.251",
                "216.127.71.252",
                "216.127.71.253",
                "216.127.71.254",
                "216.127.71.255"
            };
                return ipWhitelist.Contains(this.ipAddress);
            }


            /**
             * @return bool 
             */
            public bool isParametersValid()
            {
                int errorsNumber = 0;
                List<string> requiredParams = new List<string>();

                if (Paymentwall_Pingback.getApiType() == Paymentwall_Pingback.API_VC)
                {
                    requiredParams.AddRange(new string[] { "uid", "currency", "type", "ref", "sig" });
                }
                else if (Paymentwall_Pingback.getApiType() == Paymentwall_Pingback.API_GOODS)
                {
                    requiredParams.AddRange(new string[] { "uid", "goodsid", "type", "ref", "sig" });
                }
                else
                { //API_CART
                    requiredParams.AddRange(new string[] { "uid", "goodsid[0]", "type", "ref", "sig" });
                }

                foreach (string field in requiredParams)
                {
                    if (!this.parameters.ContainsKey(field) || String.IsNullOrWhiteSpace(this.parameters[field]))
                    {
                        this.appendToErrors("Parameter " + field + " is missing");
                        errorsNumber++;
                    }
                }

                return errorsNumber == 0;
            }


            /**
             * Get pingback parameter
             * 
             * @param string param
             * @return string
             */
            public string getParameter(string param)
            {
                if (!String.IsNullOrWhiteSpace(this.parameters[param]))
                    return this.parameters[param];
                else
                    return null;
            }


            /**
             * Get pingback parameter "type"
             * 
             * @return int
             */
            public int getPingbackType()
            {   //changed to getPingbackType() to avoid duplicate name with C# method getType()
                if (this.parameters["type"] != null)
                    return Convert.ToInt32(this.parameters["type"]);
                else
                    return -1;
            }


            /**
             * Get verbal explanation of the informational pingback
             * 
             * @return string
             */
            public string getTypeVerbal()
            {
                Dictionary<string, string> pingbackTypes = new Dictionary<string, string>();
                pingbackTypes.Add(Paymentwall_Pingback.PINGBACK_TYPE_SUBSCRIPTION_CANCELLATION.ToString(), "user_subscription_cancellation");
                pingbackTypes.Add(Paymentwall_Pingback.PINGBACK_TYPE_SUBSCRIPTION_EXPIRED.ToString(), "user_subscription_expired");
                pingbackTypes.Add(Paymentwall_Pingback.PINGBACK_TYPE_SUBSCRIPTION_PAYMENT_FAILED.ToString(), "user_subscription_payment_failed");

                if (!String.IsNullOrWhiteSpace(this.parameters["type"]))
                {
                    if (pingbackTypes.ContainsKey(this.parameters["type"]))
                        return pingbackTypes[this.parameters["type"]];
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }


            /**
             * Get pingback parameter "uid"
             * 
             * @return string
             */
            public string getUserId()
            {
                return this.getParameter("uid");
            }


            /**
             * Get pingback parameter "currency"
             * 
             * @return string
             */
            public string getVirtualCurrencyAmount()
            {
                return this.getParameter("currency");
            }


            /**
             * Get product id
             * 
             * @return string
             */
            public string getProductId()
            {
                return this.getParameter("goodsid");
            }


            /**
             * @return int
             */
            public int getProductPeriodLength()
            {
                return Convert.ToInt32(this.getParameter("slength"));
            }


            /*
             * @return string
             */
            public string getProductPeriodType()
            {
                return this.getParameter("speriod");
            }


            /*
             *  @return Paymentwall_Product 
             */
            public Paymentwall_Product getProduct()
            {
                string productType = (this.getProductPeriodLength() > 0 ? Paymentwall_Product.TYPE_SUBSCRIPTION : Paymentwall_Product.TYPE_FIXED);

                Paymentwall_Product product = new Paymentwall_Product(
                        this.getProductId(),
                        0,
                        null,
                        null,
                        productType,
                        this.getProductPeriodLength(),
                        this.getProductPeriodType()
                );

                return product;
            }


            /*
             * @return List<Paymentwall_Product>
             */
            public List<Paymentwall_Product> getProducts()
            {
                List<Paymentwall_Product> products = new List<Paymentwall_Product>();
                List<string> productIds = new List<string>();

                foreach (var productId in this.parameters["goodsid"])
                {
                    productIds.Add(productId.ToString());
                }

                if (productIds.Any())
                {
                    foreach (string id in productIds)
                    {
                        products.Add(new Paymentwall_Product(id));
                    }
                }

                return products;
            }


            /*
             * Get pingback parameter "ref"
             * 
             * @return string
             */
            public string getReferenceId()
            {
                return this.getParameter("ref");
            }


            /*
             * Returns unique identifier of the pingback that can be used for checking
             * If the same pingback was already processed by your servers
             * Two pingbacks with the same unique ID should not be processed more than once
             * 
             * @return string
             */
            public string getPingbackUniqueId()
            {
                return this.getReferenceId() + "_" + this.getPingbackType();
            }


            /*
             * Check wheter product is deliverable
             * 
             * @return bool
             */
            public bool isDeliverable()
            {
                return (
                  this.getPingbackType() == Paymentwall_Pingback.PINGBACK_TYPE_REGULAR ||
                  this.getPingbackType() == Paymentwall_Pingback.PINGBACK_TYPE_GOODWILL ||
                  this.getPingbackType() == Paymentwall_Pingback.PINGBACK_TYPE_RISK_REVIEWED_ACCEPTED
                );
            }


            /*
             * Check wheter product is cancelable
             * 
             * @return bool
             */
            public bool isCancelable()
            {
                return (
                    this.getPingbackType() == Paymentwall_Pingback.PINGBACK_TYPE_NEGATIVE ||
                    this.getPingbackType() == Paymentwall_Pingback.PINGBACK_TYPE_RISK_REVIEWED_DECLINED
                );
            }


            /*
             * Check whether product is under review
             * 
             * @return bool
             */
            public bool isUnderReview()
            {
                return this.getPingbackType() == Paymentwall_Pingback.PINGBACK_TYPE_RISK_UNDER_REVIEW;
            }


            /*
             * Build signature for the pingback received
             * 
             * @param Dictionary<string, string> parameters
             * @param string secret Paymentwall Secret Key
             * @param int version Paymentwall Signature Version
             * @return string
             */
            public string calculateSignature(Dictionary<string, string> signatureParamsToSign, string secret, int version)
            {
                string baseString = "";
                signatureParamsToSign.Remove("sig");

                if (version == Paymentwall_Pingback.SIGNATURE_VERSION_2 || version == Paymentwall_Pingback.SIGNATURE_VERSION_3)
                {
                    signatureParamsToSign = signatureParamsToSign.OrderBy(d => d.Key, StringComparer.Ordinal).ToDictionary(d => d.Key, d => d.Value);
                }

                foreach (KeyValuePair<string, string> kvp in signatureParamsToSign)
                {
                    baseString += kvp.Key + "=" + kvp.Value;
                }
                baseString += secret;

                if (version == Paymentwall_Pingback.SIGNATURE_VERSION_3)
                {
                    return Paymentwall_Pingback.getHash(baseString, "sha256");
                }
                else
                {
                    return Paymentwall_Pingback.getHash(baseString, "md5");
                }
            }


            /*
             * Generate a hased string
             * 
             * @param string inputString The string to be hased
             * @param string algorithm The hash algorithm, e.g. md5, sha256
             * @return string hashed string
             */
            private static string getHash(string inputString, string algorithm)
            {
                HashAlgorithm alg = null;

                if (algorithm == "md5")
                    alg = MD5.Create();
                else if (algorithm == "sha256")
                    alg = SHA256.Create();

                byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
        public class Paymentwall_Product
        {

            /**
             * Product types 
             */
            public const string TYPE_SUBSCRIPTION = "subscription";
            public const string TYPE_FIXED = "fixed";


            /**
             * Product period types
             */
            public const string PERIOD_TYPE_DAY = "day";
            public const string PERIOD_TYPE_WEEK = "week";
            public const string PERIOD_TYPE_MONTH = "month";
            public const string PERIOD_TYPE_YEAR = "year";


            /**
             * Paymentwall_Product class's properties
             */
            private string productId;
            private float amount;
            private string currencyCode;
            private string name;
            private string productType;
            private int periodLength;
            private string periodType;
            private bool recurring;
            private Paymentwall_Product trialProduct;


            /**
             * Default constructor
             */
            public Paymentwall_Product() { }


            /**
             * @param string productId your internal product ID, e.g. product1
             * @param float amount product price, e.g. 9.99
             * @param string currencyCode ISO currency code, e.g. USD
             * @param string name product name
             * @param string productType product type, Paymentwall_Product::TYPE_SUBSCRIPTION for recurring billing, or Paymentwall_Product::TYPE_FIXED for one-time payments
             * @param int periodLength product period length, e.g. 3. Only required if product type is subscription
             * @param string periodType product period type, e.g. Paymentwall_Product::PERIOD_TYPE_MONTH. Only required if product type is subscription
             * @param bool recurring if the product recurring
             * @param Paymentwall_Product trialProduct trial product, product type should be subscription, recurring should be True
             */
            public Paymentwall_Product(string productId, float amount = 0.0f, string currencyCode = null, string name = null, string productType = Paymentwall_Product.TYPE_FIXED, int periodLength = 0, string periodType = null, bool recurring = false, Paymentwall_Product trialProduct = null)
            {
                this.productId = productId;
                this.amount = amount;
                this.currencyCode = currencyCode;
                this.name = name;
                this.productType = productType;
                this.periodLength = periodLength;
                this.periodType = periodType;
                this.recurring = recurring;

                if (productType == Paymentwall_Product.TYPE_SUBSCRIPTION && this.recurring == true)
                {
                    this.trialProduct = trialProduct;
                }
                else
                {
                    this.trialProduct = null;
                }
            }


            /**
             * @return string product ID
             */
            public string getId()
            {
                return this.productId;
            }


            /**
             * @return float product price, e.g. 96.69
             */
            public float getAmount()
            {
                return this.amount;
            }

            /**
             * @return string ISO currency code, e.g. USD
             */
            public string getCurrencyCode()
            {
                return this.currencyCode;
            }

            /**
             * @return string product name
             */
            public string getName()
            {
                return this.name;
            }

            /**
             * @return string product type, Paymentwall_Product::TYPE_SUBSCRIPTION for recurring billing, Paymentwall_Product::TYPE_FIXED for one-time
             */
            public string getType()
            {
                return this.productType;
            }

            /**
             * @return string product period type, e.g. Paymentwall_Product::PERIOD_TYPE_MONTH
             */
            public string getPeriodType()
            {
                return this.periodType;
            }

            /**
             * @return string product period length, e.g. 3
             */
            public int getPeriodLength()
            {
                return this.periodLength;
            }

            /**
             * @return bool if the product recurring
             */
            public bool isRecurring()
            {
                return this.recurring;
            }

            /**
             * @return Paymentwall_Product trial product
             */
            public Paymentwall_Product getTrialProduct()
            {
                return this.trialProduct;
            }

        }
        public class Paymentwall_Widget : Paymentwall_Base
        {

            /**
             * Widget call URL
             */
            const string BASE_URL = "https://api.paymentwall.com/api";


            protected string userId;
            protected string widgetCode;
            protected List<Paymentwall_Product> products = new List<Paymentwall_Product>();
            protected Dictionary<string, string> extraParams = new Dictionary<string, string>();


            /*
             * @param string userId identifier of the end-user who is viewing the widget
             * @param string widgetCode e.g. p1 or p1_1, can be found inside of your Paymentwall Merchant account in the Widgets section
             * @param List<Paymentwall_Product> products array that consists of Paymentwall_Product entities; for Flexible Widget Call use array of 1 product
             * @param Dictionary<string, string> extraParams associative array of additional params that will be included into the widget URL, 
             * e.g. 'sign_version' or 'email'. Full list of parameters for each API is available at http://paymentwall.com/documentation
             */
            public Paymentwall_Widget(string userId, string widgetCode, List<Paymentwall_Product> products, Dictionary<string, string> extraParams)
            {
                this.userId = userId;
                this.widgetCode = widgetCode;
                this.products = products;
                this.extraParams = extraParams;
            }


            /*
             * Widget constructor for Virtual Currency API
             * 
             * @param string userId identifier of the end-user who is viewing the widget
             * @param string widgetCode e.g. p1 or p1_1, can be found inside of your Paymentwall Merchant account in the Widgets section
             * @param Dictionary<string, string> extraParams associative array of additional params that will be included into the widget URL, 
             * e.g. 'sign_version' or 'email'. Full list of parameters for each API is available at http://paymentwall.com/documentation
             */
            public Paymentwall_Widget(string userId, string widgetCode, Dictionary<string, string> extraParams)
            {
                this.userId = userId;
                this.widgetCode = widgetCode;
                this.extraParams = extraParams;
                this.products = new List<Paymentwall_Product>();
            }


            /*
             * Get default signature version for this API Type
             * 
             * @return int
             */
            public int getDefaultSignatureVersion()
            {
                if (Paymentwall_Widget.getApiType() != Paymentwall_Widget.API_CART)
                {
                    return Paymentwall_Widget.DEFAULT_SIGNATURE_VERSION;
                }
                else
                {
                    return Paymentwall_Widget.SIGNATURE_VERSION_2;
                }
            }


            /*
             * Return URL for the widget
             * 
             * @return string
             */
            public string getUrl()
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["key"] = Paymentwall_Widget.getAppKey();
                parameters["uid"] = this.userId;
                parameters["widget"] = this.widgetCode;

                int productsNumber = this.products.Count;

                if (Paymentwall_Widget.getApiType() == Paymentwall_Widget.API_GOODS)
                {
                    if (productsNumber > 0)
                    {
                        if (productsNumber == 1)
                        {
                            Paymentwall_Product product = this.products[0];
                            Paymentwall_Product postTrialProduct = null;
                            if (product.getTrialProduct() is Paymentwall_Product)
                            {
                                postTrialProduct = product;
                                product = product.getTrialProduct();
                            }
                            parameters.Add("amount", product.getAmount().ToString());
                            parameters.Add("currencyCode", product.getCurrencyCode());
                            parameters.Add("ag_name", product.getName());
                            parameters.Add("ag_external_id", product.getId());
                            parameters.Add("ag_type", product.getType());

                            if (product.getType() == Paymentwall_Product.TYPE_SUBSCRIPTION)
                            {
                                parameters.Add("ag_period_length", product.getPeriodLength().ToString());
                                parameters.Add("ag_period_type", product.getPeriodType());

                                if (product.isRecurring())
                                {
                                    parameters.Add("ag_recurring", (Convert.ToInt32(product.isRecurring())).ToString());

                                    if (postTrialProduct != null)
                                    {
                                        parameters.Add("ag_trial", "1");
                                        parameters.Add("ag_post_trial_external_id", postTrialProduct.getId());
                                        parameters.Add("ag_post_trial_period_length", postTrialProduct.getPeriodLength().ToString());
                                        parameters.Add("ag_post_trial_period_type", postTrialProduct.getPeriodType());
                                        parameters.Add("ag_post_trial_name", postTrialProduct.getName());
                                        parameters.Add("post_trial_amount", postTrialProduct.getAmount().ToString());
                                        parameters.Add("post_trial_currencyCode", postTrialProduct.getCurrencyCode().ToString());
                                    }

                                }

                            }

                        } //end if (productNumber == 1)

                        else
                        {
                            //TODO: Paymentwall_Widget.appendToErrors('Only 1 product is allowed in flexible widget call');
                        }

                    } //end if (productNumber > 0) 

                }
                else if (Paymentwall_Widget.getApiType() == Paymentwall_Widget.API_CART)
                {
                    int index = 0;

                    foreach (Paymentwall_Product product in this.products)
                    {
                        parameters.Add("external_ids[" + index.ToString() + "]", product.getId());

                        if (product.getAmount() != -1f)
                        {
                            parameters.Add("prices[" + index.ToString() + "]", product.getAmount().ToString());
                        }

                        if (product.getCurrencyCode() != null)
                        {
                            parameters.Add("currencies[" + index.ToString() + "]", product.getCurrencyCode());
                        }

                        if (product.getName() != null)
                        {
                            parameters.Add("names[" + index.ToString() + "]", product.getName());
                        }

                        index++;
                    }

                    index = 0;
                }

                int signatureVersion = this.getDefaultSignatureVersion();
                parameters.Add("sign_version", Convert.ToString(signatureVersion));

                if (this.extraParams.ContainsKey("sign_version"))
                {
                    parameters["sign_version"] = this.extraParams["sign_version"];
                    signatureVersion = Convert.ToInt32(this.extraParams["sign_version"]);
                }
                parameters = mergeDictionaries(parameters, extraParams);

                parameters["sign"] = Paymentwall_Widget.calculateSignature(parameters, Paymentwall_Widget.getSecretKey(), signatureVersion);

                return Paymentwall_Widget.BASE_URL + "/" + this.buildController(this.widgetCode) + "?" + this.buildQueryString(parameters, "&");
            }


            /**
             * Return HTML code for the widget
             *
             * @param Dictionary<string, string> attributes associative array of additional HTML attributes, e.g. Dictionary.Add("width", "100%")
             * @return string
             */
            public string getHtmlCode(Dictionary<string, string> attributes = null)
            {
                Dictionary<string, string> defaultAttributes = new Dictionary<string, string>();
                defaultAttributes.Add("frameborder", "0");
                defaultAttributes.Add("width", "750");
                defaultAttributes.Add("height", "800");
                if (attributes != null)
                {
                    attributes = mergeDictionaries(defaultAttributes, attributes);
                }
                else
                {
                    attributes = defaultAttributes;
                }
                var attributesQuery = this.buildQueryString(attributes, " ");
                return "<iframe src='" + this.getUrl() + "' " + attributesQuery + "></iframe>";
            }


            /**
             * Build controller URL depending on API type
             *
             * @param string widget code of the widget
             * @param bool flexibleCall
             * @return string
             */
            protected string buildController(string widget, bool flexibleCall = false)
            {
                if (Paymentwall_Widget.getApiType() == Paymentwall_Widget.API_VC)
                {
                    if (!Regex.IsMatch(widget, @"^w|s|mw"))
                    {
                        return Paymentwall_Widget.CONTROLLER_PAYMENT_VIRTUAL_CURRENCY;
                    }
                    else
                    {
                        return "";
                    }
                }
                else if (Paymentwall_Widget.getApiType() == Paymentwall_Widget.API_GOODS)
                {
                    if (!flexibleCall)
                    {
                        if (!Regex.IsMatch(widget, @"^w|s|mw"))
                        {
                            return Paymentwall_Widget.CONTROLELR_PAYMENT_DIGITAL_GOODS;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return Paymentwall_Widget.CONTROLELR_PAYMENT_DIGITAL_GOODS;
                    }
                }
                else
                {
                    return Paymentwall_Widget.CONTROLLER_PAYMENT_CART;
                }
            }


            /**
             * Build signature for the widget specified
             *
             * @param Dictionary<string, string> parameters
             * @param string secret Paymentwall Secret Key
             * @param int version Paymentwall Signature Version
             * @return string
             */
            public static string calculateSignature(Dictionary<string, string> parameters, string secret, int version)
            {
                string baseString = "";

                if (version == Paymentwall_Widget.SIGNATURE_VERSION_1)
                {   //TODO: throw exception if no uid parameter is present 

                    if (parameters["uid"] != null)
                        baseString += parameters["uid"];
                    else
                        baseString += secret;
                    return Paymentwall_Widget.getHash(baseString, "md5");
                }
                else
                {
                    parameters = parameters.OrderBy(d => d.Key, StringComparer.Ordinal).ToDictionary(d => d.Key, d => d.Value);

                    foreach (KeyValuePair<string, string> param in parameters)
                    {
                        baseString += param.Key + "=" + param.Value;
                    }
                    baseString += secret;

                    if (version == Paymentwall_Widget.SIGNATURE_VERSION_2)
                        return Paymentwall_Widget.getHash(baseString, "md5");
                    else
                        return Paymentwall_Widget.getHash(baseString, "sha256");
                }
            }


            /*
             * Build the query string
             * 
             * @param Dictionary<string, string> dict The input dictionary
             * @param string s The connector sign, e.g. &, =, or white space
             * @return string
             */
            private string buildQueryString(Dictionary<string, string> dict, string s)
            {
                var queryString = new StringBuilder();

                int count = 0;
                bool end = false;

                foreach (string key in dict.Keys)
                {
                    if (count == dict.Count - 1) end = true;

                    string escapedValue = Uri.EscapeDataString(dict[key] ?? string.Empty);
                    if (end)
                        queryString.AppendFormat("{0}={1}", key, escapedValue);
                    else
                        queryString.AppendFormat("{0}={1}{2}", key, escapedValue, s);

                    count++;
                }
                return queryString.ToString();
            }


            /*
             * Generate a hased string
             * 
             * @param string inputString The string to be hased
             * @param string algorithm The hash algorithm, e.g. md5, sha256
             * @return string hashed string
             */
            private static string getHash(string inputString, string algorithm)
            {
                HashAlgorithm alg = null;

                if (algorithm == "md5")
                    alg = MD5.Create();
                else if (algorithm == "sha256")
                    alg = SHA256.Create();

                byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }


            /**
             * Merging 2 dictionaries to 1 dictionary
             * 
             * @param dict1 Dictionary<string, string> The first dictionary
             * @param dict2 Dictionar<string, string> The second dictionary
             * @return Dictionary<string, string> The merged dictionary
             */
            private Dictionary<string, string> mergeDictionaries(Dictionary<string, string> dict1, Dictionary<string, string> dict2)
            {
                foreach (KeyValuePair<string, string> kvp in dict2)
                {
                    if (dict1.ContainsKey(kvp.Key))
                        dict1[kvp.Key] = kvp.Value;
                    else
                        dict1.Add(kvp.Key, kvp.Value);
                }
                return dict1;
            }
        }
    }
}
