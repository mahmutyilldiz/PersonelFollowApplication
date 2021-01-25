using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;//veritabanı baglantısı için

  namespace PersonelTakip
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0; Data Source=personel.accdb");
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        //form3deki datagridview1 de personel bilgileinin listelenmesi için bir metot yazalımÇ
        private void personel_listele()
        {
            try
            {
                baglantim.Open();
                //datagridview nesnesinde listeleme yapacagımız için sorgumuzun basında oldbdataadapter fonksiyonunu kullanmalıyız

                OleDbDataAdapter listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NUMARASI], ad AS[ADI],soyad AS[SOYADI]," +
                    "cinsiyet AS[CİNSİYETİ],mezuniyet AS[MEZUNİYETİ],dogum_tarihi AS[DOGUM TARİHİ],gorevi AS[GÖREVİ], gorevyeri AS[GÖREV YERİ]," +
                    "maasi AS[MAAŞI] from personeller Order By ad ASC", baglantim);

                DataSet dshafiza = new DataSet();//dataset ile sorgumuzun sonucu için bellekten alan ayırırız
                listele.Fill(dshafiza);//sorgunun sonucunu ayırdıgımız alana atadık
                dataGridView1.DataSource = dshafiza.Tables[0];//ayrılan alandaki ilk tabloyu atadık
                baglantim.Close();

            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                baglantim.Close();
               
            }
            
           
        
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            //form3 acılması: giris ekranında (form1) den yetki alanı kullancı işaretlendiğinde ve doğru kullanıcı ve sifre girildiğinde 
            //form 3 ekranı acılacaktır: yani yetkisi yönetici olanlardan bu arayüze giremeyecekler
            //ve form3 de personellerle ilgili sekmedir

            //personeller tablosundaki tüm kayıtların datagridviewdda listenlenmesi için yazdıgımız
            //personel_listele metodumuzu formun load olayında cagıralım.

            //picturebox1 ayaralama

            pictureBox1.Height = 100;
            pictureBox1.Width = 100;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;

            //picturebox2
            pictureBox2.Height = 100;
            pictureBox2.Width = 100;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;




            personel_listele();

            this.Text = "KULLANICI İŞLEMLERİ";
            label19.Text = Form1.adi + " " + Form1.soyadi;
            //giris yapan kişinn fotosunu picturebox2 de görüntüleyecegiz: veritabanından resim cekme
            try
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\" + Form1.tcno + ".jpg");  //".jpg"  uzantısını belirler isimde 
                //222222222222.jpg diye bir resim ismi yok yani. o sadece uzantısnı belirliyor

            }
            catch 
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok.jpg");


            }

        }
    }
}
