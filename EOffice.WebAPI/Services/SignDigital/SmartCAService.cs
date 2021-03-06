using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using EOffice.WebAPI.Models;
using Newtonsoft.Json;
using SignService.Common.HashSignature.Interface;
using SignService.Common.HashSignature.Pdf;
using File = System.IO.File;

namespace EOffice.WebAPI.Services.SignDigital
{

    public class SmartCA
    {
        public static ResponseMessage getSignFile(string user, string pass, string content, string fileName, byte[] file, string pageNumber, string xPosition, string yPosition)
        {

            ResponseMessage result = _signSmartCA_PDF(user, pass, content, fileName, file, pageNumber, xPosition, yPosition);
            return result;
        }
        public static ResponseMessage getSignFileTemp(string user, string pass, byte[] image, string fileName, byte[] file, string pageNumber, string xPosition, string yPosition,  KySoPhapLyModel model)
        {

            ResponseMessage result = _signSmartCA_PDFTemp(user, pass, image, fileName, file, pageNumber, xPosition, yPosition, model);
            return result;
        }
        private static ResponseMessage _signSmartCA_PDFTemp(string user, string pass, byte[] image, string fileName, byte[] file, string pageNumber, string xPosition, string yPosition, KySoPhapLyModel model)
        {
            var customerEmail = user;
            var customerPass = pass;
            ResponseMessage responseMessage = new ResponseMessage();
            //var access_token = CoreServiceClient.GetAccessToken(customerEmail, customerPass, out string refresh_token);

            var access_token = _getAccessToken("https://rmgateway.vnptit.vn/auth/token", customerEmail, customerPass, "4185-637127995547330633.apps.signserviceapi.com", "NGNhMzdmOGE-OGM2Mi00MTg0");
            if (String.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("Can get access token");
                //return;
            }

            String credential = _getCredentialSmartCA(access_token, "https://rmgateway.vnptit.vn/csc/credentials/list");
            String certBase64 = _getAccoutSmartCACert(access_token, "https://rmgateway.vnptit.vn/csc/credentials/info", credential);
            UserInfo userInfo = _getUserInfo(access_token);
            //string _pdfInput = @"C:\Users\User29421\Documents\ca\test_smartca_thien.pdf";
            //string _pdfSignedPath = @"C:\Users\User29421\Documents\ca\test_smartca_thien_signed2.pdf";

            // byte[] unsignData = null;
            byte[] unsignData = file;
            /* try
             {
                 unsignData = File.ReadAllBytes(_pdfInput);
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex);
                 return;
             }*/
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
               "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"
            );
            IHashSigner signer = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.PDF);
            signer.SetHashAlgorithm(SignService.Common.HashSignature.Common.MessageDigestAlgorithm.SHA256);


            #region Optional -----------------------------------
            // Property: L?? do k?? s???
            ((PdfHashSigner)signer).SetReason("X??c nh???n t??i li???u");
            // H??nh ???nh hi???n th??? tr??n ch??? k?? (m???c ?????nh l?? logo VNPT)
             //var imgBytes = File.ReadAllBytes("logo_vnpt.png");
             // var x = Convert.ToBase64String(imgBytes);
             ((PdfHashSigner)signer).SetCustomImage(image);
            // Signing page (@deprecated)
            //((PdfHashSigner)signer).SetSigningPage(1);
            // V??? tr?? v?? k??ch th?????c ch??? k?? (@deprecated)
            //((PdfHashSigner)signer).SetSignaturePosition(20, 20, 220, 50);
            // Ki???u hi???n th??? ch??? k?? (OPTIONAL/DEFAULT=TEXT_WITH_BACKGROUND)
            ((PdfHashSigner)signer).SetRenderingMode(PdfHashSigner.RenderMode.LOGO_ONLY);
            // N???i dung text tr??n ch??? k?? (OPTIONAL)
            // ((PdfHashSigner)signer).SetLayer2Text("K?? b???i: Subject name\nNg??y k??: Datetime.now");
            ((PdfHashSigner)signer).SetLayer2Text("");
            // Fontsize cho text tr??n ch??? k?? (OPTIONAL/DEFAULT = 10)
            ((PdfHashSigner)signer).SetFontSize(13);

            ((PdfHashSigner)signer).SetLayer2Text($"");
            // ((PdfHashSigner)signer).SetLayer2Text($"K?? b???i: {userInfo.fullName}\nNg??y k??: {DateTime.Now}");
            // M??u text tr??n ch??? k?? (OPTIONAL/DEFAULT=000000)
            ((PdfHashSigner)signer).SetFontColor("000000");
            // Ki???u ch??? tr??n ch??? k??
            ((PdfHashSigner)signer).SetFontStyle(PdfHashSigner.FontStyle.Normal);
            // Font ch??? tr??n ch??? k??
            ((PdfHashSigner)signer).SetFontName(PdfHashSigner.FontName.Times_New_Roman);

            //Hi???n th??? ch??? k?? v?? v??? tr?? b??n d?????i d??ng _textFilter c??ch 1 ??o???n _marginTop, ????? r???ng _width, ????? cao _height
            //using (var reader = new PdfReader(unsignData))
            //{

            //    var parser = new PdfReaderContentParser(reader);

            //    for (int pageNum = 1; pageNum <= reader.NumberOfPages; ++pageNum)
            //    {
            //        var strategy = parser.ProcessContent(pageNum, new LocationTextExtractionStrategyWithPosition());

            //        var res = strategy.GetLocations();

            //        var post = new TextLocation();

            //        foreach (TextLocation textContent in res)
            //        {
            //            if (textContent.Text.Contains(_textFilter))
            //            {
            //                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
            //                {
            //                    Rectangle = string.Format("{0},{1},{2},{3}", (int)textContent.X, (object)(int)(textContent.Y - _marginTop - _height), (int)(textContent.X + _width), (int)(textContent.Y - _marginTop)),
            //                    Page = pageNum
            //                });
            //            }
            //        }
            //    }



            //    reader.Close();
            //    //var searchResult = res.Where(p => p.Text.Contains(searchText)).OrderBy(p => p.Y).Reverse().ToList();
            //}            

            // Hi???n th??? ???nh ch??? k?? t???i nhi???u v??? tr?? tr??n t??i li???u

            if (!string.IsNullOrEmpty(xPosition) && !string.IsNullOrEmpty(yPosition) && !string.IsNullOrEmpty(pageNumber))
            {
                const int W = 595;
                const int H = 842;
                int x1 = (int)Math.Round(float.Parse(xPosition));
                int y1 = (int)Math.Round(float.Parse(yPosition));
                int w = (int)Math.Round(float.Parse(model.Width));
                int h = (int)Math.Round(float.Parse(model.Height));

                int d1 = x1;
                int d2 = H - (y1 + h);
                int b1 = x1 + w;
                int b2 = H - y1;

                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
                {
                    // V???i k??ch th?????c ch??? k?? 200x50
                    Rectangle = $"{d1},{d2},{b1},{b2}",
                    Page = int.Parse(pageNumber)
                });
           
            }
            else
            {

                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
                {
                    Rectangle = "220,100,220,450",
                    Page = 1
                });
            }

            //((PdfHashSigner)signer).AddSignatureComment(new PdfSignatureComment
            //{
            //    Type = (int)PdfSignatureComment.Types.TEXT,
            //    Text = "This is comment",
            //    Page = 1,
            //    Rectangle = "20,20,220,50",
            //});

            // Th??m comment v??o d??? li???u
            /* ((PdfHashSigner)signer).AddSignatureComment(new PdfSignatureComment
             {
                 Page = 1,
                 Rectangle = "92,609,292,630",
                 Text = "yahohohohohohohhods??nlfn",
                 FontName = "Times_New_Roman",
                 FontSize = 13,
                 FontColor = "0000FF",
                 FontStyle = 2,
                 Type = (int)PdfSignatureComment.Types.TEXT,
             });*/

            // Signature widget border type (OPTIONAL)
            ((PdfHashSigner)signer).SetSignatureBorderType(PdfHashSigner.VisibleSigBorder.NONE);
            #endregion -----------------------------------------
            //var hashValue = "ULUOyLMAvzuNOOVJJG/GMdIonsfkD2mtagK5R8RV3cY=";
            var hashValue = signer.GetSecondHashAsBase64();

            //var tranId = _signHash(access_token, "https://rmgateway.vnptit.vn/csc/signature/signhash", hashValue, credential);
            //SignHash End

            //Sign Begin
            var tranId = _sign(access_token, "https://rmgateway.vnptit.vn/csc/signature/sign", Convert.ToBase64String(unsignData), credential, "Kiet dep trai", fileName);
            //Sign End

            if (tranId == "")
            {
                Console.WriteLine("Ky so that bai");
            }

            var count = 0;
            var isConfirm = false;
            var datasigned = "";
            while (count < 4 && !isConfirm)
            {
                Console.WriteLine("Get TranInfo PDF lan " + count + " : ");
                //_log.Info("Get TranInfo PDF lan " + count + " : ");
                var tranInfo = _getTranInfo(access_token, "https://rmgateway.vnptit.vn/csc/credentials/gettraninfo", tranId);
                if (tranInfo != null)
                {
                    if (tranInfo.tranStatus != 1)
                    {
                        count = count + 1;
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        isConfirm = true;
                        datasigned = tranInfo.documents[0].sig;
                        responseMessage.ResponseCode = 200;
                        responseMessage.ResponseContent = "Ky so thanh cong.";
                    }
                }
                else
                {
                    Console.WriteLine("Error from content");
                    //return;
                }
            }
            if (!isConfirm)
            {
                responseMessage.ResponseCode = 408;
                responseMessage.ResponseContent = "Het han.";
                Console.WriteLine("Signer not confirm from App");
                //return;
            }

            if (string.IsNullOrEmpty(datasigned))
            {
                responseMessage.ResponseCode = 400;
                responseMessage.ResponseContent = "Loi ky so.";
                //return;
            }


            if (!signer.CheckHashSignature(datasigned))
            {
                Console.WriteLine("Signature not match");
                //return;
            }
            // ------------------------------------------------------------------------------------------

            // 3. Package external signature to signed file
            byte[] signed = signer.Sign(datasigned);
            responseMessage.Content = signed;
            //File.WriteAllBytes(_pdfSignedPath, signed);

            return responseMessage;
            //File.WriteAllBytes(_pdfSignedPath, Convert.FromBase64String(datasigned));
            // Console.WriteLine("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");
            //_log.Info("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");

        }

        private static ResponseMessage _signSmartCA_PDF(string user, string pass, string content, string fileName, byte[] file, string pageNumber, string xPosition, string yPosition)
        {
            var customerEmail = user;
            var customerPass = pass;
            ResponseMessage responseMessage = new ResponseMessage();
            //var access_token = CoreServiceClient.GetAccessToken(customerEmail, customerPass, out string refresh_token);

            var access_token = _getAccessToken("https://rmgateway.vnptit.vn/auth/token", customerEmail, customerPass, "4185-637127995547330633.apps.signserviceapi.com", "NGNhMzdmOGE-OGM2Mi00MTg0");
            if (String.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("Can get access token");
                //return;
            }

            String credential = _getCredentialSmartCA(access_token, "https://rmgateway.vnptit.vn/csc/credentials/list");
            String certBase64 = _getAccoutSmartCACert(access_token, "https://rmgateway.vnptit.vn/csc/credentials/info", credential);
            UserInfo userInfo = _getUserInfo(access_token);
            //string _pdfInput = @"C:\Users\User29421\Documents\ca\test_smartca_thien.pdf";
            //string _pdfSignedPath = @"C:\Users\User29421\Documents\ca\test_smartca_thien_signed2.pdf";

            // byte[] unsignData = null;
            byte[] unsignData = file;
            /* try
             {
                 unsignData = File.ReadAllBytes(_pdfInput);
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex);
                 return;
             }*/
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
               "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"
            );
            IHashSigner signer = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.PDF);
            signer.SetHashAlgorithm(SignService.Common.HashSignature.Common.MessageDigestAlgorithm.SHA256);


            #region Optional -----------------------------------
            // Property: L?? do k?? s???
            ((PdfHashSigner)signer).SetReason("X??c nh???n t??i li???u");
            // H??nh ???nh hi???n th??? tr??n ch??? k?? (m???c ?????nh l?? logo VNPT)
             var imgBytes = File.ReadAllBytes("logo_vnpt.png");
             // var x = Convert.ToBase64String(imgBytes);
            // ((PdfHashSigner)signer).SetCustomImage(imgBytes);
            // Signing page (@deprecated)
            //((PdfHashSigner)signer).SetSigningPage(1);
            // V??? tr?? v?? k??ch th?????c ch??? k?? (@deprecated)
            //((PdfHashSigner)signer).SetSignaturePosition(20, 20, 220, 50);
            // Ki???u hi???n th??? ch??? k?? (OPTIONAL/DEFAULT=TEXT_WITH_BACKGROUND)
            ((PdfHashSigner)signer).SetRenderingMode(PdfHashSigner.RenderMode.LOGO_ONLY);
            // N???i dung text tr??n ch??? k?? (OPTIONAL)
            // ((PdfHashSigner)signer).SetLayer2Text("K?? b???i: Subject name\nNg??y k??: Datetime.now");
            ((PdfHashSigner)signer).SetLayer2Text("");
            // Fontsize cho text tr??n ch??? k?? (OPTIONAL/DEFAULT = 10)
            ((PdfHashSigner)signer).SetFontSize(13);

            ((PdfHashSigner)signer).SetLayer2Text($"");
            // ((PdfHashSigner)signer).SetLayer2Text($"K?? b???i: {userInfo.fullName}\nNg??y k??: {DateTime.Now}");
            // M??u text tr??n ch??? k?? (OPTIONAL/DEFAULT=000000)
            ((PdfHashSigner)signer).SetFontColor("000000");
            // Ki???u ch??? tr??n ch??? k??
            ((PdfHashSigner)signer).SetFontStyle(PdfHashSigner.FontStyle.Normal);
            // Font ch??? tr??n ch??? k??
            ((PdfHashSigner)signer).SetFontName(PdfHashSigner.FontName.Times_New_Roman);

            //Hi???n th??? ch??? k?? v?? v??? tr?? b??n d?????i d??ng _textFilter c??ch 1 ??o???n _marginTop, ????? r???ng _width, ????? cao _height
            //using (var reader = new PdfReader(unsignData))
            //{

            //    var parser = new PdfReaderContentParser(reader);

            //    for (int pageNum = 1; pageNum <= reader.NumberOfPages; ++pageNum)
            //    {
            //        var strategy = parser.ProcessContent(pageNum, new LocationTextExtractionStrategyWithPosition());

            //        var res = strategy.GetLocations();

            //        var post = new TextLocation();

            //        foreach (TextLocation textContent in res)
            //        {
            //            if (textContent.Text.Contains(_textFilter))
            //            {
            //                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
            //                {
            //                    Rectangle = string.Format("{0},{1},{2},{3}", (int)textContent.X, (object)(int)(textContent.Y - _marginTop - _height), (int)(textContent.X + _width), (int)(textContent.Y - _marginTop)),
            //                    Page = pageNum
            //                });
            //            }
            //        }
            //    }



            //    reader.Close();
            //    //var searchResult = res.Where(p => p.Text.Contains(searchText)).OrderBy(p => p.Y).Reverse().ToList();
            //}            

            // Hi???n th??? ???nh ch??? k?? t???i nhi???u v??? tr?? tr??n t??i li???u

            if (!string.IsNullOrEmpty(xPosition) && !string.IsNullOrEmpty(yPosition) && !string.IsNullOrEmpty(pageNumber))
            {
                int x1 = int.Parse(xPosition);
                int y1 = int.Parse(yPosition);
                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
                {
                    // V???i k??ch th?????c ch??? k?? 200x50
                    Rectangle = $"{x1 - 100},{y1 - 25},{x1 + 100},{y1 + 25}",
                    Page = int.Parse(pageNumber)
                });

            }
            else
            {

                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
                {
                    Rectangle = "20,100,220,150",
                    Page = 1
                });
            }

            //((PdfHashSigner)signer).AddSignatureComment(new PdfSignatureComment
            //{
            //    Type = (int)PdfSignatureComment.Types.TEXT,
            //    Text = "This is comment",
            //    Page = 1,
            //    Rectangle = "20,20,220,50",
            //});

            // Th??m comment v??o d??? li???u
            /* ((PdfHashSigner)signer).AddSignatureComment(new PdfSignatureComment
             {
                 Page = 1,
                 Rectangle = "92,609,292,630",
                 Text = "yahohohohohohohhods??nlfn",
                 FontName = "Times_New_Roman",
                 FontSize = 13,
                 FontColor = "0000FF",
                 FontStyle = 2,
                 Type = (int)PdfSignatureComment.Types.TEXT,
             });*/

            // Signature widget border type (OPTIONAL)
            ((PdfHashSigner)signer).SetSignatureBorderType(PdfHashSigner.VisibleSigBorder.DASHED);
            #endregion -----------------------------------------
            //var hashValue = "ULUOyLMAvzuNOOVJJG/GMdIonsfkD2mtagK5R8RV3cY=";
            var hashValue = signer.GetSecondHashAsBase64();

            //var tranId = _signHash(access_token, "https://rmgateway.vnptit.vn/csc/signature/signhash", hashValue, credential);
            //SignHash End

            //Sign Begin
            var tranId = _sign(access_token, "https://rmgateway.vnptit.vn/csc/signature/sign", Convert.ToBase64String(unsignData), credential, content, fileName);
            //Sign End

            if (tranId == "")
            {
                Console.WriteLine("Ky so that bai");
            }

            var count = 0;
            var isConfirm = false;
            var datasigned = "";
            while (count < 4 && !isConfirm)
            {
                Console.WriteLine("Get TranInfo PDF lan " + count + " : ");
                //_log.Info("Get TranInfo PDF lan " + count + " : ");
                var tranInfo = _getTranInfo(access_token, "https://rmgateway.vnptit.vn/csc/credentials/gettraninfo", tranId);
                if (tranInfo != null)
                {
                    if (tranInfo.tranStatus != 1)
                    {
                        count = count + 1;
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        isConfirm = true;
                        datasigned = tranInfo.documents[0].sig;
                        responseMessage.ResponseCode = 200;
                        responseMessage.ResponseContent = "Ky so thanh cong.";
                    }
                }
                else
                {
                    Console.WriteLine("Error from content");
                    //return;
                }
            }
            if (!isConfirm)
            {
                responseMessage.ResponseCode = 408;
                responseMessage.ResponseContent = "Het han.";
                Console.WriteLine("Signer not confirm from App");
                //return;
            }

            if (string.IsNullOrEmpty(datasigned))
            {
                responseMessage.ResponseCode = 400;
                responseMessage.ResponseContent = "Loi ky so.";
                //return;
            }


            if (!signer.CheckHashSignature(datasigned))
            {
                Console.WriteLine("Signature not match");
                //return;
            }
            // ------------------------------------------------------------------------------------------

            // 3. Package external signature to signed file
            byte[] signed = signer.Sign(datasigned);
            responseMessage.Content = signed;
            //File.WriteAllBytes(_pdfSignedPath, signed);

            return responseMessage;
            //File.WriteAllBytes(_pdfSignedPath, Convert.FromBase64String(datasigned));
            // Console.WriteLine("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");
            //_log.Info("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");

        }

        private static string _getCredentialSmartCA(String accessToken, String serviceUri)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", "{}");

                    CredentialSmartCAResponse credentials = JsonConvert.DeserializeObject<CredentialSmartCAResponse>(HtmlResult);
                    if (credentials != null)
                    {
                        return credentials.content[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }
        private static String _getAccoutSmartCACert(String accessToken, String serviceUri, string credentialId)
        {
            var req = new ReqCertificateSmartCA
            {
                credentialId = credentialId,
                certificates = "single",
                certInfo = true,
                authInfo = true
            };
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    CertificateSmartCAResponse res = JsonConvert.DeserializeObject<CertificateSmartCAResponse>(HtmlResult);
                    String certBase64 = res.cert.certificates[0];
                    return certBase64.Replace("\r\n", "");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        private static TranInfoSmartCARespContent _getTranInfo(string accessToken, String serviceUri, String tranId)
        {

            var req = new ContenSignHash
            {
                tranId = tranId
            };
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    TranInfoSmartCAResp resp = JsonConvert.DeserializeObject<TranInfoSmartCAResp>(HtmlResult);
                    if (resp.code == 0)
                    {
                        return resp.content;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        private static string _signHash(string accessToken, String serviceUri, string data, string credentialId)
        {

            var req = new SignHashSmartCAReq
            {
                credentialId = credentialId,
                refTranId = "1234-5678-90000",
                notifyUrl = "http://10.169.0.221/api/v1/smart_ca_callback",
                description = "Test for office",
                datas = new List<DataSignHash>()
            };
            var test = new DataSignHash
            {
                name = "test.docx",
                hash = data
            };
            req.datas.Add(test);
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    SignHashSmartCAResponse resp = JsonConvert.DeserializeObject<SignHashSmartCAResponse>(HtmlResult);
                    if (resp.code == 0)
                    {
                        return resp.content.tranId;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }
        private static string _sign(string accessToken, String serviceUri, string data, string credentialId, string content, string fileName)
        {

            var req = new SignSmartCAReq
            {
                credentialId = credentialId,
                refTranId = "1234-5678-90000",
                notifyUrl = "http://10.169.0.221/api/v1/smart_ca_callback",
                description = content,
                datas = new List<DataSign>()
            };
            var test = new DataSign
            {
                name = fileName,
                dataBase64 = data
            };
            req.datas.Add(test);
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    SignHashSmartCAResponse resp = JsonConvert.DeserializeObject<SignHashSmartCAResponse>(HtmlResult);
                    if (resp.code == 0)
                    {
                        return resp.content.tranId;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        private static string _getAccessToken(String uri, String user, String pass, string client_id, string client_secret)
        {
            string URI = uri;
            String body = $"username={user}&password={pass}&client_id={client_id}&client_secret={client_secret}&grant_type=password";
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, "POST", body);
                    var content = JsonConvert.DeserializeObject<GetTokenResponse>(HtmlResult);
                    return content.access_token;
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
            return null;
        }

        private static UserInfo _getUserInfo(string accessToken)
        {
            String serviceUri = "https://rmgateway.vnptit.vn/identityapi/userinfo/info";
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST");

                    UserInfoResponse resp = JsonConvert.DeserializeObject<UserInfoResponse>(HtmlResult);
                    if (resp.code == 0)
                    {
                        return resp.content;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        class GetTokenResponse
        {
            // access_token value
            public string access_token { get; set; }
            // refresh_token to get new access_token (see RefreshToken method)
            public string refresh_token { get; set; }
            public string token_type { get; set; }
            // access_token valid time. when expired, using refresh_token to get new or require user re-authorize
            public int expires_in { get; set; }
        }
    }
}