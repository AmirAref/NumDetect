using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace NumDetect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // variables
        public int Target { get; set; }
        public int Steps { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // disable playing at startup
            disable_playing();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // check numbers
            Regex pattern = new Regex("[^0-9]+");
            e.Handled = pattern.IsMatch(e.Text);

            if (!e.Handled)
            {
                // check length
                e.Handled = ((TextBox)sender).Text.Length >= 5;
                if (e.Handled)
                {
                    MessageBox.Show("تعداد رقم ها بیش از حد مجاز است !", "ورودی غیر مجاز", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // pass the slider value to the text box 
            txt_range.Text = ((int)e.NewValue).ToString();
        }

        private void txt_range_TextChanged(object sender, TextChangedEventArgs e)
        {
            // check empty text
            if (txt_range.Text.Length < 1)
            {
                return;
            }

            // check higher than 1000
            int range = int.Parse(txt_range.Text);
            if (range > 1000)
            {
                range = 1000;
                MessageBox.Show("محدوده ی اعداد نمیتواند بیشتر از 1000 باشد !", "تعداد غیرمجاز", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // pass the text box value to the slider
            txt_range.Text = range.ToString();
            Slider.Value = (double)range;
        }

        private void txt_steps_TextChanged(object sender, TextChangedEventArgs e)
        {
            // check empty text
            if (string.IsNullOrWhiteSpace(txt_steps.Text))
            {
                return;
            }

            // set the steps
            Steps = int.Parse(txt_steps.Text);

        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            // disable playing items
            disable_playing();
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            // check the steps field
            if (txt_steps.Text.Length < 1)
            {
                MessageBox.Show("ابتدا فیلد تعداد دفعات را با یک عدد پر کنید !", "فیلد را پر کنید", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // check the steps field
            if (txt_range.Text.Length < 1)
            {
                MessageBox.Show("ابتدا محدوده ی اعداد را انتخاب کنید !", "فیلد را پر کنید", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // check the maximum of steps
            int range = (int)Slider.Value;
            int steps = int.Parse(txt_steps.Text);
            if (steps > range)
            {
                MessageBox.Show("تعداد دفعات نمی تواند از محدوده ی اعداد بزرگتر باشد !", "تعداد غیرمجاز", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // generate a random number in the range
            Random rand = new Random();
            this.Target = rand.Next(1, range + 1);

            // enable playing items
            enable_playing();
            // display the rest steps
            lbl_steps.Content = "فرصت های باقی مانده : " + Steps.ToString();

        }

        private void btn_guss_Click(object sender, RoutedEventArgs e)
        {
            // check empty text
            if (txt_guss.Text.Length < 1) {            
                MessageBox.Show("ابتدا حدس خود را وارد کنید !", "فیلد را پر کنید", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // hgher than maximum range
            int guss = int.Parse(txt_guss.Text);
            if (guss > Slider.Value)
            {
                MessageBox.Show(string.Format("لطفا یک عدد در محدوده ی 1 تا {0} وارد کنید !", txt_range.Text), "فیلد را پر کنید", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

                // decrease the step
                Steps -= 1;

            // display the rest steps
            lbl_steps.Content = "فرصت های باقی مانده : " + Steps.ToString();


            // compare with target
            if (guss > Target)
            {
                // higher than target
                lbl_status.Content = "حدس شما بزرگ است !";

            }
            else if (guss < Target)
            {
                // lower than target
                lbl_status.Content = "حدس شما کوچک است !";
            }
            else if (guss == Target)
            {
                // equals with target
                MessageBox.Show("شما برنده شدید !", "برد", MessageBoxButton.OK, MessageBoxImage.Information);
                disable_playing();
                return;
            }
            // check steps 
            if (Steps == 0)
            {
                // equals with target
                MessageBox.Show("شما باختید !\nعدد مورد نظر : " + Target.ToString(), "باخت", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                disable_playing();
                return;
            }
        }


        private void enable_playing()
        {
            // re assign the steps value
            this.Steps = int.Parse(txt_steps.Text);

            // enable the start game items
            txt_guss.IsEnabled = true;
            btn_Reset.IsEnabled = true;
            btn_guss.IsEnabled = true;
            // disable the playing items
            txt_range.IsEnabled = false;
            txt_steps.IsEnabled = false;
            Slider.IsEnabled = false;
            btn_Start.IsEnabled = false;

            // enable blur effect for setting items
            txt_range.Effect = new System.Windows.Media.Effects.BlurEffect();
            txt_steps.Effect = new System.Windows.Media.Effects.BlurEffect();
            Slider.Effect = new System.Windows.Media.Effects.BlurEffect();
            btn_Start.Effect = new System.Windows.Media.Effects.BlurEffect();
            lbl_select.Effect = new System.Windows.Media.Effects.BlurEffect();
            lbl_select_steps.Effect = new System.Windows.Media.Effects.BlurEffect();
            // disable blur effect for playing items
            txt_guss.Effect = null;
            btn_guss.Effect = null;
            btn_Reset.Effect = null;
            lbl_guss.Effect = null;
        }

        private void disable_playing()
        {
            // clear the boxes and reset to default
            txt_guss.Clear();
            lbl_status.Content = "";
            lbl_steps.Content = "";

            // enable the start game items
            txt_range.IsEnabled = true;
            txt_steps.IsEnabled = true;
            Slider.IsEnabled = true;
            btn_Start.IsEnabled = true;
            // disable the playing items
            txt_guss.IsEnabled = false;
            btn_Reset.IsEnabled = false;
            btn_guss.IsEnabled = false;

            // enable blur effect for playing items
            txt_guss.Effect = new System.Windows.Media.Effects.BlurEffect();
            btn_Reset.Effect = new System.Windows.Media.Effects.BlurEffect();
            btn_guss.Effect = new System.Windows.Media.Effects.BlurEffect();
            lbl_guss.Effect = new System.Windows.Media.Effects.BlurEffect();
            // disable blur effect for setting items
            txt_steps.Effect = null;
            txt_range.Effect = null;
            Slider.Effect = null;
            btn_Start.Effect = null;
            lbl_select.Effect = null;
            lbl_select_steps.Effect = null;
        }

    }
}
