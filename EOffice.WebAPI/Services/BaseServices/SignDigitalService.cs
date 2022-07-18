using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Xml;
using EOffice.WebAPI.Models;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Formatting;
using Spire.Pdf;
using FileFormat = Spire.Doc.FileFormat;

namespace EOffice.WebAPI.Services
{
    public static class SignDigitalService
    {
        public static void GeneratSignKey(ref User model)
        {
            RsaKeyPairGenerator rsaKey = new RsaKeyPairGenerator();
            rsaKey.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKey.GenerateKeyPair();

            RsaKeyParameters Private_Key = (RsaKeyParameters) keyPair.Private;
            RsaKeyParameters Public_Key = (RsaKeyParameters) keyPair.Public;

            model.PrivateKey_string = PrivateKeytoString(Private_Key);
            model.PublicKey_string = PublicKeytoString(Public_Key);
        }

        private static string PublicKeytoString(RsaKeyParameters _key)
        {
            byte[] publicKeyDer = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(_key).GetDerEncoded();
            String publicKeyDerBase64 = Convert.ToBase64String(publicKeyDer);
            return publicKeyDerBase64;
        }

        public static RsaKeyParameters StringtoPrivateKey(string keyString)
        {
            byte[] PrivateKeyDerRestored = Convert.FromBase64String(keyString);
            RsaKeyParameters PrivateKeyRestored = (RsaKeyParameters) PrivateKeyFactory.CreateKey(PrivateKeyDerRestored);
            return PrivateKeyRestored;
        }

        public static string PrivateKeytoString(RsaKeyParameters _key)
        {
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(_key);
            // Write out an RSA private key with it's asscociated information as described in PKCS8.
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            // Convert to Base64 ..
            string serializedPrivateString = Convert.ToBase64String(serializedPrivateBytes);
            return serializedPrivateString;
        }

        public static string ExtractWord(string filepath)
        {
            //Load Document
            Document document = new Document();
            //document.LoadFromFile(@"D:\Doc3.doc");
            document.LoadFromFile(filepath);

            //Initialzie StringBuilder Instance
            StringBuilder sb = new StringBuilder();

            sb.Append(document.GetText());

            //Extract Text from Word and Save to StringBuilder Instance
            //foreach (Section section in document.Sections)
            //{
            //    foreach (Paragraph paragraph in section.Paragraphs)
            //    {
            //        sb.AppendLine(paragraph.Text);
            //    }
            //}

            //Create a New TXT File to Save Extracted Text
            //File.WriteAllText(@"D:\TextFromWord.txt", sb.ToString());
            //System.Diagnostics.Process.Start("ExtractText.txt");

            string str = sb.ToString();
            str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return str;
            //Console.WriteLine("Trich xuat noi Word dung thanh cong!");
        }

        public static string ExtractPDF(string path)
        {
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(path);

            StringBuilder content = new StringBuilder();
            content.Append(document.Pages[0].ExtractText());
            // content.Append(document.Pages[1].ExtractText());

            //String fileName = @"D:\TextFromPDF.txt";
            //File.WriteAllText(fileName, content.ToString());

            string str = content.ToString();
            str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return str;
            //System.Diagnostics.Process.Start("TextFromPDF.txt");

            // Console.WriteLine("Trich xuat noi dung PDF thanh cong!");
        }
    }

    public class KySoNoiBoService
    {
        public void TienTrinhKySo(string pathWord, string fileName, string pathPDF, List<User> users)
        {
            Console.WriteLine(".............................");
            Console.WriteLine(".......Demo Sign Digital.....");
            Console.WriteLine(".............................");

            //So luong User
            int Num_user = users.Count;

            //document File(

            byte[] tmpSource;


            //Lay noi dung file word
            string TextFromWord = SignDigitalService.ExtractWord(pathWord);
            Console.WriteLine();
            Console.WriteLine("Noi dung file word...");
            Console.WriteLine();
            Console.WriteLine(TextFromWord);
            Console.WriteLine();
            Console.WriteLine("Do dai noi dung file word: " + TextFromWord.Length);
            tmpSource = ASCIIEncoding.ASCII.GetBytes(TextFromWord);


            // Tao List chua cac ket qua ma hoa noi dung va chu ky

            List<string> listDSign = new List<string>();
            for (int i = 0; i < Num_user; i++)
            {
                ISigner signP = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id);
                signP.Init(true, SignDigitalService.StringtoPrivateKey(users[i].PrivateKey_string));
                signP.BlockUpdate(tmpSource, 0, tmpSource.Length);
                byte[] SignD = signP.GenerateSignature();

                //listDSign.Add(listUser[i].name + Hex.ToHexString(SignD)); // '\u00cb'.ToString()
                listDSign.Add(users[i].FullName + Convert.ToBase64String(SignD)); // '\u00cb'.ToString()
            }

            string StartSignD = "startSignD";
            string EndSignD = "endSignD";
            var chukyso = StartSignD + string.Join("##", listDSign) + EndSignD;

            // chen vao word và xuất ra file pdf
            MultiInsertToDoc(chukyso, pathWord, pathPDF, fileName, users);

            // //xac thu chu ky
            // ValidDsign(pathFilePDF, listUser[0]);
        }

        public void MultiInsertToDoc(string p, string pathWord, string pathPDF, string fileName, List<User> listuser)
        {
            Document doc = new Document();
            doc.LoadFromFile(pathWord, FileFormat.Doc);

            //Section sec = doc.AddSection();// them moi section
            Section sec = doc.LastSection; // khong them moi sextion

            //dinh dang chu ky so
            CharacterFormat formatDSign = new CharacterFormat(doc);
            formatDSign.FontName = "Calibri";
            formatDSign.FontSize = 2;
            formatDSign.Bold = false;
            formatDSign.TextColor = Color.White; // chu~ Trang se~ an chu ky so


            //danh dau ket thuc noi dung vb
            Paragraph parEnd = sec.AddParagraph();
            //TextBox textBoxEnd = parEnd.AppendTextBox(400, 20);
            //Paragraph EndText = textBoxEnd.Body.AddParagraph();
            // EndText.AppendText('\u00cb'.ToString()).ApplyCharacterFormat(formatSign);
            parEnd.AppendText(p).ApplyCharacterFormat(formatDSign);
            //textBoxEnd.Format.NoLine = true;

            //chen cac o ky ten
            //Paragraph par = sec.AddParagraph();
            //Dinh dang thong tin nguoi ky
            CharacterFormat formatHSign = new CharacterFormat(doc);
            formatHSign.FontSize = 12;
            formatHSign.Bold = false;


            int num_row = 0;

            //them chu ky vao vb
            if (listuser.Count % 2 == 0)
            {
                num_row = listuser.Count / 2;
            }
            else
            {
                num_row = listuser.Count / 2 + 1;
            }


            Table table = sec.AddTable(true);
            // table.TableFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.None;
            table.ResetCells(num_row, 2); // set so dong/cot

            //table.ApplyHorizontalMerge(1,0, 1); //merge cell

            int i = 0;
            for (int r = 0; r < num_row; r++)
            {
                TableRow DataRow = table.Rows[r];

                //C Represents Column 1.
                //Cell Alignment

                //Fill Data in Rows
                Paragraph p2 = DataRow.Cells[0].AddParagraph();
                p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                Image image = Image.FromFile(listuser[i].FilePath);
                p2.AppendText("Chữ ký hợp lệ \n").ApplyCharacterFormat(formatHSign);
                p2.AppendText(listuser[i].FullName + "\n").ApplyCharacterFormat(formatHSign);
                p2.AppendText(listuser[i].DonVi?.Ten + "\n").ApplyCharacterFormat(formatHSign);
                p2.AppendText(listuser[i].NgayKy + "\n").ApplyCharacterFormat(formatHSign);
                DocPicture pic2 = p2.AppendPicture(image);
                pic2.Height = pic2.Height * 0.3f;
                pic2.Width = pic2.Width * 0.2f;
                // DataRow.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle ;

                i = i + 1;

                //if (listuser.Count%2!=0 && r== listuser.Count-2)
                if (i > listuser.Count - 1 && listuser.Count % 2 != 0)
                {
                    table.ApplyHorizontalMerge(r, 0, 1); //merge cell
                }
                else
                {
                    //C Represents Column 2.
                    //DataRow.Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    //Fill Data in Rows
                    Paragraph p3 = DataRow.Cells[1].AddParagraph();
                    p3.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    p3.AppendText("Chữ ký hợp lệ \n").ApplyCharacterFormat(formatHSign);
                    p3.AppendText(listuser[i].FullName + "\n").ApplyCharacterFormat(formatHSign);
                    p3.AppendText(listuser[i].DonVi?.Ten + "\n").ApplyCharacterFormat(formatHSign);
                    p3.AppendText(listuser[i].NgayKy + "\n").ApplyCharacterFormat(formatHSign);
                    DocPicture pic3 = p3.AppendPicture(image);

                    pic3.Height = pic3.Height * 0.3f;
                    pic3.Width = pic3.Width * 0.2f;
                    i = i + 1;
                }
            }


            // doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
            //
            try
            {
                doc.SaveToFile(pathPDF, FileFormat.PDF);
                // using (MemoryStream ms = new MemoryStream())
                // {
                //
                //     doc.SaveToFile(ms, FileFormat.Doc);
                //     doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                //     File.WriteAllBytes(pathPDF, ms.ToArray());
                // }
                // doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                // using (var strem = System.IO.File.Create(pathPDF))
                // {
                //     // strem.Position = 0;
                //     doc.SaveToStream(strem, Spire.Doc.FileFormat.PDF);
                // }
                // doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                // using (var stream = new FileStream
                //            (pathPDF, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                // {
                //     stream.Position = 0;
                //     doc.SaveToStream(stream, Spire.Doc.FileFormat.PDF);
                //     stream.CopyToAsync()
                // }
                // doc.SaveToFile(pathPDF);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("Ghi File thanh cong!");

            //mo file
            Process pr = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.UseShellExecute = true;
            pi.FileName = pathPDF;
            pr.StartInfo = pi;

            try
            {
                pr.Start();
            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
            }
        }
    }
}