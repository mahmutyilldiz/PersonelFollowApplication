using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;//veritabanı baglantısı
using System.Text.RegularExpressions;//güvenli parola olusturulmasını saglayan kütüphanedir:(regex)
using System.IO;//giris-cıkıs işlemlerine ait:(bir klasörün olup olmadıgını kontrolü)

namespace PersonelTakip
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        //1.ADIM:veritabanı baglantısının yapılması
        //2.ADIM:form2 ekranı acıldıgı anda tabpage1 ekranımdaki datagridview1 nesneme kullanicilar tablosundaki kayıtların listelenmesi

        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=personel.accdb");

        //2.adımı bir metot tanımlayark yapıyorum. cünkü kaydet,sil,güncelle olaylarında sürekli olarak listeleme islemini yapacagım 
        //için bir metot yazarak kod uzunlugunu önlemiş oluyoruum!

        private void kullanicilar_listele()
        {
            try
            {
                baglantim.Open();
                //DGV de bir sütünun istediğim gibi gözükmesi için "tcno AS[TC KİMLİK NO]"-> DGVİEWDA TC KİMLİK NO sütun adı olacak
                OleDbDataAdapter kullanicilar_goster = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],yetki AS[YETKİ],kullaniciadi AS[KULLANICI ADI],parola AS[PAROLA] " +
                    "from kullanicilar Order By ad ASC", baglantim);

                DataSet dshafiza = new DataSet();//bellekten sorgu sonucunu tutmak için alan ayırıyoruz
                kullanicilar_goster.Fill(dshafiza);//sorgumuzu veritabanına işledik: yani- ayrılan alana kullanicilar tablosunu atadık
                dataGridView1.DataSource = dshafiza.Tables[0];//sorguyu dataGV e atıyoruz
                baglantim.Close();


            }
            catch (Exception hatamsj)
            {

                MessageBox.Show(hatamsj.Message, "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();//bir hata olusması durumunda sonraki kod satırlarımızn etkilenmemesi için kapatalım database.
            } 
        


        
        
        }

        //3.ADIM:form ekranı acıldıgında tabpage2 ekranını tasarladım önceden. datagridview2 nesneme  personeller 
        //tablosundaki kayıtları listelenmesini yine bir metot tanımlayarak yapıyorum

        private void personeller_listele()
        {


            try
            {
                baglantim.Open();

                OleDbDataAdapter personeller_goster = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],cinsiyet AS[CİNSİYETİ],mezuniyet AS[MEZUNİYETİ],dogum_tarihi AS[DOGUM TARİHİ],gorevi AS[GOREVİ],gorevyeri AS[GÖREV YERİ],maasi AS[MAAŞI ] from personeller", baglantim); ;
                DataSet dshafiza = new DataSet();//bellekten sorgu sonucunu tutmak için alan ayırıyoruz
                personeller_goster.Fill(dshafiza);//sorgumuzu veritabanına işledik: yani- ayrılan alana kullanicilar tablosunu atadık
                dataGridView2.DataSource = dshafiza.Tables[0];//sorguyu dataGV e atıyoruz
                baglantim.Close();


            }
            catch (Exception hatamsj)
            {

                MessageBox.Show(hatamsj.Message, "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();//bir hata olusması durumunda sonraki kod satırlarımızn etkilenmemesi için kapatalım database.
            }






        }


        private void Form2_Load(object sender, EventArgs e)
        {
            //FORM2 ayarlarını yapalım

            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//yüklenilen resim pictureboxu kaplasın:
            
                try
                {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\" + Form1.tcno + ".jpg");
                }

                catch
                {

                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok");


                }

                
            //KULLANICI İŞLEMLERİ SEKMESİ 

            this.Text = "YÖNETİCİ İŞEMLERİ";
            label11.ForeColor = Color.DarkRed;
            label11.Text = Form1.adi + " " + Form1.soyadi;//form1 de giris yapan kişinin ad,soyad,tcno,yetki gibi bilgileri public static degiskenlerde tutuluyor. ve her formdan erisiliyor
            textBox1.MaxLength = 11;//tc no 
            textBox4.MaxLength = 8;//kullanıcı adı
            radioButton1.Checked = true;//form calıstıgunda otomatik secili
            toolTip1.IsBalloon = true;
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.AutomaticDelay = 1000;
            toolTip1.SetToolTip(this.textBox1, "TC kimlik no 11 karakterden olusmalıdır!");//tc kimlik uyarı kiçin tooltip nesnesi kullanıldı
            textBox2.CharacterCasing = CharacterCasing.Upper;//bu texte girilen karakterleri büyük harfe cevirir:
            textBox3.CharacterCasing = CharacterCasing.Upper;
            textBox5.MaxLength = 10;
            textBox6.MaxLength = 10;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Step = 10;
            kullanicilar_listele();


            //PERSONEL İŞLEMLERİ SEKMESİ

            //picturebox nesnesini ayarlıyoruz

            pictureBox2.BorderStyle = BorderStyle.Fixed3D;//cercevesi 3 boyutlu olarak gözüksün:
            pictureBox2.Height = 100;pictureBox2.Width = 100;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;//yüklenilen resim picturebox'ı kaplasınnnn:

            //maskedtextbox: kullanıcının bizim istedigimiz kriterlerde veri girmesine zorluyoruz

            maskedTextBox1.Mask = "00000000000";//0 demek zorunlu rakam girisi: burda 11 tane zorunku rakam girisi yapılsın istiyoruz
            maskedTextBox2.Mask = "LL???????????????";//min 2 karakter zorunlu, enfazla ise 17 karakter girilebilir:
            maskedTextBox3.Mask = "LL???????????????";
            maskedTextBox4.Mask = "0000";    //min asgari ücret alındıgını max ise 9999 maas alındıgın varsayalım. zorunlu 4 rakam girisi
            maskedTextBox4.Text = "0";// if ile maasının 1000 den küçük olma durumunu kıyaslama yaptıgımız için ilk deger olarak 0 ataması yapmak zorundayız
            //yani bos oldugunda hata almayı önlemek için
            maskedTextBox2.Text.ToUpper();//büuük harfe dönustur
            maskedTextBox3.Text.ToUpper();

            //combobaxlara deger ataması yapalım:

            string[] dizi1 = { "İlköğretim", "Ortaöğretim", "Lise", "Üniversite" };
            comboBox1.Items.AddRange(dizi1);//combobox1 için:mezuniyet

            string[] dizi2 = { "Yönetici", "Memur", "Şöfür", "İşçi"};
            comboBox2.Items.AddRange(dizi2);//görevi

            string[] dizi3 = { "Arge", "Bilgi İşlem", "Muhasebe", "Üretim", "Paketleme", "Nakliye" };
            comboBox3.Items.AddRange(dizi3);//görev yeri

            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            //datetimepicker nesnesini ayaralayalım:

            DateTime zaman = DateTime.Now;//bir zaman degiskeni tanımladık anlık zamanı atadık:
            int yil = int.Parse(zaman.ToString("yyyy"));//zamanı stringe cevir ve sadece yıl kısmınıı al ve degiskene ata!!yıl:yyyy
            int ay = int.Parse(zaman.ToString("MM"));//ay:MM
            int gun = int.Parse(zaman.ToString("dd"));//gun:dd

            dateTimePicker1.MinDate = new DateTime(1960, 1, 1);//en yaslı calısan 1960 dogumlu sınır:
            dateTimePicker1.MaxDate = new DateTime(yil - 18, ay, gun);//en genc calısan 18 yasında olsun:
            dateTimePicker1.Format = DateTimePickerFormat.Short;//kısa tarih gözüksün 

            radioButton3.Checked = true;//bay otomatik secili gelsin:
            personeller_listele();//personel sekmesi acıldıgında datagridview nesnesinde personeller tablosundaki kayıtlar listelensin
            
        }



        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //daha önce form_load kısmında bu textin max uzunlugu 11 ayarlamıstık. simdi ise 11den az girilirse error p. ile hata verecegiz
            if (textBox1.Text.Length<11)
         
                errorProvider1.SetError(textBox1, "Tc Kimlik No 11 karakter olmalı!");
               
            else
    
                errorProvider1.Clear();
 
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textbox1 aktifken sadece rakam ve backsapce tusuna basıldıgında deger girilebilecek!
            //onun dısında klavyeden basılsada deger girisi izin verilmeyecek:ASCII:48-57 ve backspace=8 asccı kodudur

            if (((int)e.KeyChar>=48&&(int)e.KeyChar<=57)||(int)e.KeyChar==8)//(int)e.keychar>=....
           
                e.Handled = false;//giris tusu aktif olsun
            else
            
                e.Handled = true;//farklı degere izin verilmesin
            
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece harf,bosluk ve backspace tuslarını aktif edelim//

            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)

                e.Handled = false;
            else

                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece harf,bosluk ve backspace tuslarını aktif edelim

            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)

                e.Handled = false;
            else

                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sadece harf,rakam,bosluk ve geritusu 
            if (char.IsLetter(e.KeyChar) == true || char.IsDigit(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)

                e.Handled = false;
            else
                e.Handled = true;

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length != 8)

                errorProvider1.SetError(textBox4, "Kullanıcı adı 8 karakterli olmalıdır!");
            else
                errorProvider1.Clear();

        }

        int parola_skoru = 0;
        //
        //regex kütüphanesi ile güvenli parola olustuma kısmı 

        private void textBox5_TextChanged(object sender, EventArgs e)     //Güvenli Parola olusturmak için asagıdaki kodlar yazılmıstır!//regex kütüphanesi eklenmistir
        {
           

            string parola_seviyesi ="";//girdiğimiz degerlere göre zayıf,güclü,cok güclü degerleri alacatır
            //parolamızda harf,rakam ve sembol kontrolü yapmamzı sagalayacak degiskenler
            int kucuk_harf_skoru = 0, buyuk_harf_skoru = 0, rakam_skoru = 0, sembol_skoru = 0;

            string sifre = textBox5.Text;
            string duzeltilmis_sifre = "";
            duzeltilmis_sifre = sifre;
            //girilen türkce karakterleri ingizilizce karakterlere cevirmek için öncelikle aldıgımız sifreyi baska
            //bir stringe atayarak o sitringdeki türkce kar. var ise dönüşüm yapıyoruz

            duzeltilmis_sifre = duzeltilmis_sifre.Replace("İ", "I");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("ı", "i");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("Ç", "C");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("ç", "c");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("Ş", "S");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("ş", "s");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("Ğ", "G");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("ğ", "g");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("Ö", "O");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("ö", "o");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("Ü", "U");
            duzeltilmis_sifre = duzeltilmis_sifre.Replace("ü", "u");

            if (sifre!=duzeltilmis_sifre)// burada sifre kontrolü yapılmıstır!!!
            {
                sifre = duzeltilmis_sifre;
                textBox5.Text = sifre;
                MessageBox.Show("Girdiğiniz Türkçe karakter düzeltilmistir!");
            }

            //1 küçük harf  10 puan, 2 ve üzeri 20 puan !

            //sifredeki küçük harfleri bulup onları bosluk ile (bir nevi silme) yerdegistirme yapacaktır. ve kalan stringin uzunlugunu sifreninkinden cıkaracaktır
            int az_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;
            kucuk_harf_skoru = Math.Min(2, az_karakter_sayisi) * 10;//kücük harf sayısı enfazla 2 olabilir veya 1 veya 0; 2 defazla olsa da 2 kabul edilir ve 10 ile carpılır

            //1 büyük harf  10 puan, 2 ve üzeri 20 puan !
            int AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;
            buyuk_harf_skoru = Math.Min(2, AZ_karakter_sayisi) * 10;

            //1 rakam  10 puan, 2 ve üzeri 20 puan !
            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length;
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10;

            int sembol_sayisi = sifre.Length - az_karakter_sayisi - AZ_karakter_sayisi - rakam_sayisi;//regex kütüphanesi kullanmaya gerek yok.
            sembol_skoru = Math.Min(2, sembol_sayisi) * 10;

            //bizim max parola uzunlugumuz 10 du: burda ise bir kişinin kh,bh,rakam,sembol kullanarak toplamda 80 puan toplamasını sagladık yani min 8 karakter girmesini sagladık
            //progressbar nesnnemiz 100 üzerinden oldugu için 9 ve 10 karakter girilmesi kosullarını da kendimiz otomatik puan eklemsi yapalım:

       
            parola_skoru = kucuk_harf_skoru + buyuk_harf_skoru + sembol_skoru + rakam_skoru;
            if (sifre.Length == 9)

                parola_skoru += 10;
            else if (sifre.Length == 10)

                parola_skoru += 20;

            //küçükharf,buyukharf,sembolve rakam bu 4 karakterin de en az 1 kere parola da kullanılma durumunu kontrol edelim

            if (kucuk_harf_skoru==0||buyuk_harf_skoru==0||rakam_skoru==0||sembol_skoru==0)// en az bir tanesi bile kullanılmamıs ise
            {
                label22.Text = "Parolanızda mutlaka küçükharf,büyükharf,sembol ve rakam kullanmalısınız!!";

            }
            if (kucuk_harf_skoru!=0&&buyuk_harf_skoru!=0&&rakam_skoru!=0&&sembol_skoru!=0)//hepsinden en az bir kere kullanılmalıdır:
            {
                label22.Text = "";
            }

            //şimdi ise parolamızın seviyesini belirleyip ilgilidegiskene atamayapalım

            if (parola_skoru<70)
            {
                parola_seviyesi = "parola seviyesi zayıf";
            }
            else if (parola_skoru==70 || parola_skoru==80)
            {
                parola_seviyesi = "parola seviyesi güçlü";
            }
            else if (parola_skoru==90||parola_skoru==100)
            {
                parola_seviyesi = "parola seviyesi çok güçlüdür";
            }

            label9.Text = "%" + Convert.ToString(parola_skoru);
            label10.Text = parola_seviyesi;
            progressBar1.Value = parola_skoru;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)  //parola ile parola_tekrar aynı mı kontrolü yapıldı!
        {
            if (textBox6.Text!=textBox5.Text)
     
                errorProvider1.SetError(textBox6, "Parola ile Parola tekrar uyusmamaktadır!!");
            else
                errorProvider1.Clear();
           
        }   

        private void topPage1_temizle()      //kullanıcı işlemleri sekmesini temizleme metodu!
        {
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
            textBox4.Clear(); textBox5.Clear(); textBox6.Clear();

        }

        private void topPage2_temizle()  //personel işlemleri sekmesini temizleme metodu!
        {

            pictureBox2.Image = null;
            maskedTextBox1.Clear();maskedTextBox2.Clear();maskedTextBox3.Clear();maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }   

        private void button2_Click(object sender, EventArgs e)
        {
            //yeni bir kullanıcının kayıt işlemi yapılacak!!

            string yetki = "";// yetki kısmındaki degerimizitutacak

            bool kayitkontrol = false;//girilen tc veritabanında kayıtlı mı?
                                      //yoksa false döndür

            baglantim.Open();

            // bu sorgu textbox1girilen tc veritabanında daha önce kayıtlımı!
            OleDbCommand selectsorgusu = new OleDbCommand("select *from kullanicilar where tcno='" + textBox1.Text + "'", baglantim);

            OleDbDataReader kayitokuma = selectsorgusu.ExecuteReader();//sorguuyu calıstırıp sonucu kayitokuma degiskenine atadık
            while (kayitokuma.Read())   ///kayıt varsa okuma yapar!!
            {
                kayitkontrol = true;
                break;
            
            }

            baglantim.Close();

           

            if (kayitkontrol==false)    //egerki aynı tc ile daha önce kayıt girisi yapılmamıs ise;
            {
                //tcno veri kontrolü
                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;

                //ad veri konrolü
                if (textBox2.Text.Length <2 || textBox2.Text == "")
                    label2.ForeColor = Color.Red;
                else
                    label2.ForeColor = Color.Black;

                //soyadı veri konrolü
                if (textBox3.Text.Length < 2 || textBox3.Text == "")
                    label3.ForeColor = Color.Red;
                else
                    label3.ForeColor = Color.Black;

                //kullanıcıadı veri konrolü
                if (textBox4.Text.Length!=8 || textBox4.Text == "")
                    label5.ForeColor = Color.Red;
                else
                    label5.ForeColor = Color.Black;


                //parola veri konrolü
                if (parola_skoru<70 || textBox5.Text == "")
                    label6.ForeColor = Color.Red;
                else
                    label6.ForeColor = Color.Black;

                //parola_tekrar veri konrolü
                if (textBox5.Text!=textBox6.Text || textBox6.Text == "")
                    label7.ForeColor = Color.Red;
                else
                    label7.ForeColor = Color.Black;



                //genel bir kontrol yaptıktan sonra kayıt işlemine baslayabiliriz//form elemanlarını genel bir kontrol yaptık

                if (textBox1.Text.Length==11 && textBox1.Text!="" && textBox2.Text!="" && textBox2.Text.Length>1 && textBox3.Text!="" && textBox3.Text.Length>1
                    && textBox4.Text!="" &&textBox5.Text!="" && textBox6.Text!="" && textBox6.Text==textBox5.Text && parola_skoru>=70)
                {
                    if (radioButton1.Checked == true)
                        yetki = "Yönetici";
                    else if (radioButton2.Checked == true)
                        yetki = "Kullanıcı";



                    ///ARTIK VERİTABANINA KAYIT İŞLEMİNE BASLIYABİLİRİZ...
                    ///

                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into kullanicilar values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + yetki + "','" + textBox4.Text + "','" + textBox5.Text + "')", baglantim);
                        eklekomutu.ExecuteNonQuery();
                        baglantim.Close();

                        MessageBox.Show("Yeni kullanıcı kaydı olusturuldu", "YILDIZ Personel Takip Otamasyonu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        topPage1_temizle();
                    }
                    catch (Exception hatamsj)
                    {

                        MessageBox.Show(hatamsj.Message);
                        baglantim.Close();
                    }


                }

                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları gözden geciriniz", "YILDIZ Personel Takip Otamasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            else
            {
                MessageBox.Show("Girilen TC Kimlik sistemde mevcuttur!", "YILDIZ Personel Takip Otamasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }      ///yeni kayıt ekleme

        private void button1_Click(object sender, EventArgs e)   //tc kimlik no ile kayıt arama işlemi::

        {

            bool kayit_arama_durumu = false;// aradıgımız kayıt yoksa degerimiz false

            if (textBox1.Text.Length==11)
            {

                baglantim.Open();
                OleDbCommand selectsorgusu = new OleDbCommand("select *from kullanicilar where tcno='" + textBox1.Text + "'", baglantim);
                //girilen tcno ait kayıt mevcut ise kaydı sec
                OleDbDataReader kayit_okuma = selectsorgusu.ExecuteReader();
                while (kayit_okuma.Read())
                {
                    kayit_arama_durumu = true;//aradıgımız kayıt var
                    textBox2.Text = kayit_okuma.GetValue(1).ToString();//kayit_okuma degiskeninde tutulan kaydın 1.indisi=2.elemanı adı textine atalım
                    textBox3.Text = kayit_okuma.GetValue(2).ToString();
                    if (kayit_okuma.GetValue(3).ToString() == "Yönetici")
                    
                        radioButton1.Checked = true;
            
                    else
                        radioButton2.Checked = true;

                    textBox4.Text = kayit_okuma.GetValue(4).ToString();
                    textBox5.Text = kayit_okuma.GetValue(5).ToString();
                    textBox6.Text = kayit_okuma.GetValue(5).ToString();

                    break;

                }

                if (kayit_arama_durumu==false)
                {

                    MessageBox.Show("Girilen TCKimlik numarasına ait kayıt mevcut degildir", "YILDIZ Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                }

                baglantim.Close();



            }

            else
            {
                MessageBox.Show("Lutfen 11 karakter içeren bir TC degeri giriniz", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage1_temizle();
                     
            }
        }

        private void button3_Click(object sender, EventArgs e)    //tc kimlik no ile mevcut kaydın güncelleme işlemi!!
        {
            string yetki = "";// yetki kısmındaki degerimizi tutacak

          
                //tcno veri kontrolü
                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;

                //ad veri konrolü
                if (textBox2.Text.Length < 2 || textBox2.Text == "")
                    label2.ForeColor = Color.Red;
                else
                    label2.ForeColor = Color.Black;

                //soyadı veri konrolü
                if (textBox3.Text.Length < 2 || textBox3.Text == "")
                    label3.ForeColor = Color.Red;
                else
                    label3.ForeColor = Color.Black;

                //kullanıcıadı veri konrolü
                if (textBox4.Text.Length != 8 || textBox4.Text == "")
                    label5.ForeColor = Color.Red;
                else
                    label5.ForeColor = Color.Black;


                //parola veri konrolü
                if (parola_skoru < 70 || textBox5.Text == "")
                    label6.ForeColor = Color.Red;
                else
                    label6.ForeColor = Color.Black;

                //parola_tekrar veri konrolü
                if (textBox5.Text != textBox6.Text || textBox6.Text == "")
                    label7.ForeColor = Color.Red;
                else
                    label7.ForeColor = Color.Black;



                //genel bir kontrol yaptıktan sonra kayıt işlemine baslayabiliriz//form elemanlarını genel bir kontrol yaptık

                if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text != "" && textBox2.Text.Length > 1 && textBox3.Text != "" && textBox3.Text.Length > 1
                    && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && textBox6.Text == textBox5.Text && parola_skoru >= 70)
                {
                    if (radioButton1.Checked == true)
                        yetki = "Yönetici";
                    else if (radioButton2.Checked == true)
                        yetki = "Kullanıcı";



                    ///ARTIK VERİTABANINA GÜNCELLEME İŞLEMİNE BASLIYABİLİRİZ...
                    ///

                    try
                    {
                        baglantim.Open();
                    OleDbCommand guncelle_komutu = new OleDbCommand("update kullanicilar set ad='" + textBox2.Text + "',soyad='" + textBox3.Text + "',yetki='" + yetki + "',kullaniciadi='" + textBox4.Text + "',parola='" + textBox5.Text + "'  where tcno='" + textBox1.Text + "'", baglantim);
                          guncelle_komutu.ExecuteNonQuery();
                         baglantim.Close();

                        MessageBox.Show("Mevcut kayıt basarılı bir sekilde güncellestirildi", "YILDIZ Personel Takip Otamasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        topPage1_temizle();
                        kullanicilar_listele();//datagridviewa kullanicilar tablosundaki kayıtları listele methodumuz 
                    }
                    catch (Exception hatamsj)
                    {

                        MessageBox.Show(hatamsj.Message);
                        baglantim.Close();
                    }


                }

                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları gözden geciriniz", "YILDIZ Personel Takip Otamasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        private void button4_Click(object sender, EventArgs e)  //tc kimlik no ile mevcut kaydın silinmesi!!
        {


            if (textBox1.Text.Length == 11)
            {
                bool arama_kayıt_durumu = false;
                baglantim.Open();
                //ilk basta arama komutu ile kaydı bulmalıyız sonra silme komutunu gerceklestiririz!

                OleDbCommand select_sorgusu = new OleDbCommand("select *from kullanicilar where tcno='" + textBox1.Text + "'", baglantim);
                OleDbDataReader okuma_kaydi = select_sorgusu.ExecuteReader();
                while (okuma_kaydi.Read())
                {
                    arama_kayıt_durumu = true;//girilen tc ait kayıt bulunmussa;

                    OleDbCommand silme_sorgusu = new OleDbCommand("delete from kullanicilar where tcno='" + textBox1.Text + "'", baglantim);
                    silme_sorgusu.ExecuteNonQuery();
                    MessageBox.Show("Aranan kayıt silindi", "YILDIZ Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    kullanicilar_listele();
                    topPage1_temizle();
                    break;
                }


                if (arama_kayıt_durumu == false)
                {
                    MessageBox.Show("Aranan kayıt bulunamadı ve silinemedi", "YILDIZ Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                    topPage1_temizle();

                }


            }
            else
                MessageBox.Show("Lütfen 11 haneli bir TC kimlik numarası giriniz!", "YILDIZ Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void button6_Click(object sender, EventArgs e)       //GÖZAT butonunun içeriği:
        {
            //openfiledialog () nesnesi kullanarak bir resim dosyasının pictureboxa yüklenmesi
            //butona tıklandıgında resim secilip pictureboxa yüklenecek

            OpenFileDialog resimsec = new OpenFileDialog();
            //resimsec nesnesi openfiledialog nesnesinin tüm özelliklerini tasır
            //bir fonksiyon,method her neyse nesnesini tanımlamak demek bütün  özelliklerini tasıyan yeni bir nesne tanımlamıs oluruz
            resimsec.Title = "Personel resmi seciniz";//önümüze gelen ekrann baslıgı
            resimsec.Filter = "JPG Dosyalar (*.jpg) |*.jpg|JPEG Dosyalar (*.jpeg)|*.jpeg"; //secme ekranındasadece jpg,jpeg uzantılı dosyalar acılacak

            if (resimsec.ShowDialog()==DialogResult.OK)   //yani resimsecme ekranı acıldıysa

            {
                this.pictureBox2.Image = new Bitmap(resimsec.OpenFile());
                //pb2ye yeni bir resim nesnesini tanımlayarak resimsec nesnesindeki
                //resmi atadık

            }


           
        }


        //PERSONEL İŞLEMLERİ SEKMESİ KAYDET BUTONU

        private void button8_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            bool kayit_kontrol = false;

            baglantim.Open();

            OleDbCommand select_sorgusu = new OleDbCommand("select *from personeller where tcno='" + maskedTextBox1.Text + "'", baglantim);
            OleDbDataReader kayit_okuma = select_sorgusu.ExecuteReader();
            while (kayit_okuma.Read())///kayit_okuma.read()==true ->ikisde aynı işlevi yapar
            {
                kayit_kontrol = true;
                break;

              
            }

            baglantim.Close();

            if (kayit_kontrol == false)  ///girilen tc kimlik numarsına ait bir kayıt personeller tablosunda yok ise:

            {
                //kaydet tusuna basıldıgında pb2 bos ise gözat butonu kırmızı ile yazılmıs olacak
                if (pictureBox2.Image == null)
                    button6.ForeColor = Color.Red;
                else
                    button6.ForeColor = Color.Black;

                //maskedTB1mizin maskenlenmesi tamamlanmadıysa yani bu conrolümüze 11 tane rakam girisi yapılmadıysa      
                if (maskedTextBox1.MaskCompleted == false)
                    label13.ForeColor = Color.Red;
                else
                    label13.ForeColor = Color.Black;


                if (maskedTextBox2.MaskCompleted == false)
                    label14.ForeColor = Color.Red;
                else
                    label14.ForeColor = Color.Black;

                if (maskedTextBox3.MaskCompleted == false)
                    label15.ForeColor = Color.Red;
                else
                    label15.ForeColor = Color.Black;

                if (comboBox1.Text == "")
                    label17.ForeColor = Color.Red;
                else
                    label17.ForeColor = Color.Black;

                if (comboBox2.Text == "")
                    label19.ForeColor = Color.Red;
                else
                    label19.ForeColor = Color.Black;

                if (comboBox3.Text == "")
                    label20.ForeColor = Color.Red;
                else
                    label20.ForeColor = Color.Black;

                if (maskedTextBox4.MaskCompleted == false)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;

                if (int.Parse(maskedTextBox4.Text) < 1000)

                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;



                //ARTIK YENİ BİR PERSONEL KAYDI OLUSTURABİLİRİZ!

                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false
                    && maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && maskedTextBox4.MaskCompleted != false)
                {

                    if (radioButton3.Checked == true)

                        cinsiyet = "Bay";
                    else if (radioButton4.Checked == true)
                        cinsiyet = "Bayan";

                    try
                    {

                        baglantim.Open();
                        OleDbCommand ekle_komutu = new OleDbCommand("insert into personeller values('" + maskedTextBox1.Text + "','" + maskedTextBox2.Text + "','" + maskedTextBox3.Text + "'" +
                            ",'" + cinsiyet + "','" + comboBox1.Text + "','" + dateTimePicker1.Text + "','" + comboBox2.Text + "','" + comboBox3.Text + "','" + maskedTextBox4.Text + "')", baglantim);
                        ekle_komutu.ExecuteNonQuery();
                        baglantim.Close();

                        //picturebox2 nesnesine personel resmini bilgisayarın herhangi bir konumundan yükledik. bunu aynı zamanda debug klasörünün içindeki personelresimler 
                        //klasörünün içine de kaydetmemiz lazım:

                        if (!Directory.Exists(Application.StartupPath + "\\personelresimler")) //personelresimler klasörümüz yok ise

                            Directory.CreateDirectory(Application.StartupPath + "\\personelresimler");         ///klasörü olusturduk

                        //var ise resmi gidip bu klasörün içine kaydetmek
                        pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\" + maskedTextBox1.Text + ".jpg");  

                        MessageBox.Show("Yeni bir personel kaydı olusturuldu", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        personeller_listele();
                        topPage2_temizle();
                        maskedTextBox4.Text = "0";

                    }
                    catch (Exception hatamsj)
                    {

                        MessageBox.Show(hatamsj.Message, "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }






                }

                else
                    MessageBox.Show("Lütfen kırmızı alanları gözden geciriniz", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);





            }

            //girilen tc daha önce sistemde kayıtlı ise

            else
                MessageBox.Show("Girilen TC kimlige  ait kayıt bulumaktadır", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



        }
        //PERSONEL İŞLEMLERİ SİL BUTONU
        private void button10_Click(object sender, EventArgs e)
        {
            //bool mantıksal operatörünün tanımlanma amacı girilen tc ye sahip bir kayıt var mı onun kontrolünü yapmamıza yarar.
         
            if (maskedTextBox1.MaskCompleted==true)
            {
                bool kayit_arama_durumu = false;   //girilen tc sistemde kayıtlı degil

                baglantim.Open();
                //girilen tc ile veritabanında alttaki sorgu arama  yapar
                OleDbCommand arama_komutu = new OleDbCommand("select *from personeller where tcno='" + maskedTextBox1.Text + "'", baglantim);
                OleDbDataReader okuma_kaydi = arama_komutu.ExecuteReader();
                while (okuma_kaydi.Read())
                {
                    kayit_arama_durumu = true;

                    OleDbCommand delete_sorgusu = new OleDbCommand("delete from personeller where tcno='" + maskedTextBox1.Text + "'", baglantim);

                    delete_sorgusu.ExecuteNonQuery();
                    break;
                }

                  if (kayit_arama_durumu == false)
                     MessageBox.Show("Girilen Tc kimlik numarasına ait bir kayıt bulunamadı", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     baglantim.Close();
                     topPage2_temizle();
                     maskedTextBox4.Text = "0";
                
                }

            else
                  MessageBox.Show("Lütfen 11 haneli bir TC kimlik numarası giriniz", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                      topPage2_temizle();
                      maskedTextBox4.Text = "0";


        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }


        //PERSONEL İŞLEMLERİ SEKMESİ GÜNCELLEME BUTONU
        private void button9_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            

            
                //kaydet tusuna basıldıgında pb2 bos ise gözat butonu kırmızı ile yazılmıs olacak
                if (pictureBox2.Image == null)
                    button6.ForeColor = Color.Red;
                else
                    button6.ForeColor = Color.Black;

                //maskedTB1mizin maskenlenmesi tamamlanmadıysa yani bu conrolümüze 11 tane rakam girisi yapılmadıysa      
                if (maskedTextBox1.MaskCompleted == false)
                    label13.ForeColor = Color.Red;
                else
                    label13.ForeColor = Color.Black;


                if (maskedTextBox2.MaskCompleted == false)
                    label14.ForeColor = Color.Red;
                else
                    label14.ForeColor = Color.Black;

                if (maskedTextBox3.MaskCompleted == false)
                    label15.ForeColor = Color.Red;
                else
                    label15.ForeColor = Color.Black;

                if (comboBox1.Text == "")
                    label17.ForeColor = Color.Red;
                else
                    label17.ForeColor = Color.Black;

                if (comboBox2.Text == "")
                    label19.ForeColor = Color.Red;
                else
                    label19.ForeColor = Color.Black;

                if (comboBox3.Text == "")
                    label20.ForeColor = Color.Red;
                else
                    label20.ForeColor = Color.Black;

                if (maskedTextBox4.MaskCompleted == false)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;

                if (int.Parse(maskedTextBox4.Text) < 1000)

                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;



                //ARTIK MEVCUT KAYIT ÜZERİNDE DEĞİŞİKLİKLER YAPABİLİRİZ!

                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false
                    && maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "" && maskedTextBox4.MaskCompleted != false)
                {

                    if (radioButton3.Checked == true)

                        cinsiyet = "Bay";
                    else if (radioButton4.Checked == true)
                        cinsiyet = "Bayan";

                    try
                    {

                        baglantim.Open();
                    OleDbCommand guncelle_komutu = new OleDbCommand("update personeller set ad='" + maskedTextBox2.Text + "', soyad='" + maskedTextBox3.Text + "', cinsiyet='" + cinsiyet + "'" +
                        ", mezuniyet='" + comboBox1.Text + "',dogum_tarihi='" + dateTimePicker1.Text + "',gorevi='" + comboBox2.Text + "',gorevyeri='" + comboBox3.Text + "',maasi='" + maskedTextBox4.Text + "' where tcno='"+maskedTextBox1.Text+"'", baglantim);

                        guncelle_komutu.ExecuteNonQuery();
                        baglantim.Close();

                        //picturebox2 nesnesine personel resmini bilgisayarın herhangi bir konumundan yükledik. bunu aynı zamanda debug klasörünün içindeki personelresimler 
                        //klasörünün içine de kaydetmemiz lazım:

                      /* if (!Directory.Exists(Application.StartupPath + "\\personelresimler")) //personelresimler klasörümüz yok ise

                            Directory.CreateDirectory(Application.StartupPath + "\\personelresimler");         ///klasörü olusturduk

                        //var ise resmi gidip bu klasörün içine kaydetmek
                        pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\" + maskedTextBox1.Text + ".jpg");*/

                        MessageBox.Show("Mevcut kayıt güncellestirildi!", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        personeller_listele();
                        topPage2_temizle();
                        maskedTextBox4.Text = "0";

                    }
                    catch (Exception hatamsj)
                    {

                        MessageBox.Show(hatamsj.Message, "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }


                }

            else
            {
                MessageBox.Show("Yazı rengi kırmızı alanları gözden geciriniz", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                   





            }

           

           
     

        private void maskedTextBox4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)   
        {
           

            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length==11)
            {
                baglantim.Open();
                OleDbCommand select_sorgusu = new OleDbCommand("select *from personeller where tcno='" + maskedTextBox1.Text + "'", baglantim);

                OleDbDataReader okuma_kaydi = select_sorgusu.ExecuteReader();

                while (okuma_kaydi.Read())
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        //personelresimleri klasörümüzden tc bilsigini giridğimiz personelin resmini pictureboz2 de gösterdik
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" + okuma_kaydi.GetValue(0).ToString() + ".jpg");  //  yazz



                    }
                    catch
                    {
                        
                        //kişinin resmi veritabanıda yoksa resimyok adlı resmi pictureboxda gösterdik
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyok.jpg");
                            

                    }
                    //aynı sekilde diger control elemanlarına da veritabanımızdaki degerleri sıralı sekilde atamasını yapıyoruz

                    maskedTextBox2.Text = okuma_kaydi.GetValue(1).ToString();
                    maskedTextBox3.Text = okuma_kaydi.GetValue(2).ToString();
                    if (okuma_kaydi.GetValue(3).ToString() == "Bay")
                        radioButton3.Checked = true;
                    else
                        radioButton4.Checked = true;
                    comboBox1.Text = okuma_kaydi.GetValue(4).ToString();
                    dateTimePicker1.Text = okuma_kaydi.GetValue(5).ToString();
                    comboBox2.Text = okuma_kaydi.GetValue(6).ToString();
                    comboBox3.Text = okuma_kaydi.GetValue(7).ToString();
                    maskedTextBox4.Text = okuma_kaydi.GetValue(8).ToString();

                    break;
                }


                if (kayit_arama_durumu == false)

                    MessageBox.Show("Aranan kayıt bulunumadaı", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    

            }

            else

                    MessageBox.Show("Lütfen 11 haneli bir TC Kimlik NUmarası giriniz!", "YILDIZ Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                


        } //personeller sekmesinde kayıt arama. burda veritabanından resimde cekmiş oluyoruz

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }
    }
   }

