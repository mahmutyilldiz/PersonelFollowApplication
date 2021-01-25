using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;//kütüphanemizi ekledik

namespace PersonelTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //database baglantısıın yapılması
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=personel.accdb");

        //formlar arasında veri iletiminde kullanılacak degiskenler

        public static string tcno, adi, soyadi, yetki;

        // sadece bu(form1) de kullacagım degiskenler



        int hak = 3; bool durum = false; //kullanıcı adı ve parola dogru giris yapılma durumunukontrolünü saglar


        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = button1;//entere basıldıgında hangi buton çalıssın?
            this.CancelButton = button2;//esc basıldgında hangi buton çalıssın?
            label5.Text = Convert.ToString(hak);
            radioButton1.Checked = true;//form calıstgında rd1 otomatik işaretli olsun!
            this.StartPosition = FormStartPosition.CenterScreen;//form calıstında ekranın ortasına konumlandır:
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;//form calıstıgında sağüst kösedeki simgeler pasif olsun:





        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (hak!= 0)//giris hakkı varsa
            {
                baglantim.Open();
                OleDbCommand selectsorgusu = new OleDbCommand("select *from kullanicilar", baglantim);
                OleDbDataReader kayitokuma = selectsorgusu.ExecuteReader();//sorguyu calıstır,sorgu sonuclarını bellekte kayitokuma nesnnesinde tut. alan ayırımını datareader ile yaptık

                while (kayitokuma.Read()) //bellekte tutulan kayitlari oku,eger varsa!
                {

                    //GİRİS BUTONUNA BASILDIGINDA FORM2 EKRANI ACILDI(degerlerin kontrolu yapaıldı)
                    
                        if (radioButton1.Checked == true)   //ve radiobuton1 secili ise;
                    {

                        if (kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString() == textBox2.Text && kayitokuma["yetki"].ToString() == "Yönetici")//forma girilen degerlerle eslesen kayıt varsa;
                        {
                            //dogru bir kullanici girisi yapılmıstır! ve asagıdaki kod satırları  calısmaya baslar!!
                            durum = true;        //basarili giris yapılmıssa
                            tcno = kayitokuma.GetValue(0).ToString();//bulunan kayıttaki degerleri, static degiskenlerimize atıyoruz
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();

                            this.Hide();//form1 gizle
                            Form2 frm2 = new Form2();//form2 nesnesi oluturuyoruz:
                            frm2.Show();
                            break; //istenilen kayıt bulunduktan sonra tekrar tekrar bosa arama yapılmasın diye döngüyü kapatıyoruz!!

                        }





                    }

                    //GİRİS BUTONUNA BASILDIGINDA FORM3 EKRANI ACILDI(degerlerin kontrolu yapaıldı)

                        if (radioButton2.Checked == true)   //ve radiobuton1 secili ise;
                    {

                        if (kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString() == textBox2.Text && kayitokuma["yetki"].ToString() == "Kullanıcı")//forma girilen degerlerle eslesen kayıt varsa;
                        {
                            //dogru bir kullanici girisi yapılmıstır! ve asagıdaki kod satırları  calısmaya baslar!!
                            durum = true;        //basarili giris yapılmıssa
                            tcno = kayitokuma.GetValue(0).ToString();//bulunan kayıttaki degerleri, static degiskenlerimize atıyoruz
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();

                            this.Hide();//form1 gizle
                            Form3 frm3 = new Form3();//form2 nesnesi oluturuyoruz:
                            frm3.Show();
                            break; //istenilen kayıt bulunduktan sonra tekrar tekrar bosa arama yapılmasın diye döngüyü kapatıyoruz!!

                        }





                    }


                    
                }

                //eksik veya yanlıs giris yapıldıysa durum degiskeni degismeyecek ve false olarak kalıp koontrolu  yapılıp hak degeri 1 azalacak kullanıcının!
                if (durum == false)
                    hak--;
                 baglantim.Close();




            }
            label5.Text = Convert.ToString(hak);
            if (hak==0)
            {
                button1.Enabled = false;//kullanicinin gris hakkı kalmadıysa giris butonununa basamasın!
                MessageBox.Show("GİRİS HAKKINIZ KALMADI!", "YILDIZ PERSONEL TAKİP OTOMASYONU", (MessageBoxButtons.OK), MessageBoxIcon.Error);
                this.Close();//formumuzu kapattık!
            }


        }



        }

        
    }

