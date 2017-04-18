using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EnsembleDecisionTree
{
    public partial class Form1 : Form
    {
        private DataTable training_set;
        private DataColumn column;
        private int total,class_no;
        private string file_name = @"result2.txt";
        private string file_out = @"output.txt";
        private string param_file = @"data.txt";
        private string prob_name = @"weather.txt";
        private StreamReader file_stream;
        private FileStream filereader;
        private const int rule_no = 200000;//// maximam number of rule
        private const int arr_size = 200000;//// maximum array size or record number
        private const int  max_attr = 20000;/// maximum number of attributes
        private string[] output;
        private string[] class_list;
        
        double[] gain_ratio_x = new double[max_attr];
        struct trainee_info
        {
            public string problem_name;
            public int discrete;
            public int cont;
            public int no_class;
        };
        trainee_info sample_info;
        public Form1()
        {
            InitializeComponent();
            training_set = new DataTable("training_set");
            column = new DataColumn();
            file_stream = new StreamReader(prob_name);
            
            //sample_info.discrete = 2;
            //sample_info.cont = 0;
            //sample_info.no_class = 2;
            //sample_info.problem_name = "weather";
              class_list=new string[100];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {



        }
        private void  set_tableParam(string param_f)
        {
            prob_name = txt_file_name.Text.Trim();
            StreamReader param_stream = new StreamReader(param_f);
            while (file_stream.Peek() != -1)
            {

                string rec = param_stream.ReadLine();
                //Console.WriteLine(rec);
                string[] split_rec = split_record(rec);
                if (split_rec[0] == prob_name)
                {
                    sample_info.problem_name =split_rec[0];
                    //Console.WriteLine(split_rec[0] + " " + sample_info.problem_name);
                    sample_info.discrete = int.Parse(split_rec[1]);
                    //Console.WriteLine(split_rec[1] + " " + sample_info.discrete);
                    sample_info.cont = int.Parse(split_rec[2]);
                    //Console.WriteLine(split_rec[2] + " " + sample_info.cont);
                    sample_info.no_class = int.Parse(split_rec[3]);
                    //Console.WriteLine(split_rec[3] + " " + sample_info.no_class);
                   
                    break;
                }
            }
        
        }
        /// <summary>
        /// load the sample data from the file provided
        /// sample trainee set is loaded into a datatable
        /// </summary>
        /// 
       //ok

        private void create_table()
        {
            //set_tableParam(param_file);//
            if (sample_info.discrete != 0)
            {
                for (int i = 0; i < sample_info.discrete; i++)
                {
                    column = training_set.Columns.Add("col" + i.ToString());
                    column.DataType = typeof(string);
                }
            }
            if (sample_info.cont != 0)
            {
                for (int i = 0; i < sample_info.cont; i++)
                {
                    column = training_set.Columns.Add("col" + (sample_info.discrete + i).ToString());
                    column.DataType = typeof(float);
                }
            }
            column = training_set.Columns.Add("result" );
            column.DataType = typeof(string);
            
        }
        /// <summary>
        /// 
        /// </summary>
        ///
        //ok
        private void load_sample()
        { 
            int no_col=sample_info.discrete + sample_info.cont + 1;
            int k=0;
            object[] value_set=new object[no_col];
            //string[] value_set = new string[no_col];
            while(file_stream.Peek()!=-1)
            {
                string record = file_stream.ReadLine();
                //Console.WriteLine(record);
                string[] split_array = split_record(record);
                for (int i = 0; i < no_col; i++)
                {
                    value_set[i] = split_array[i];
                     
                }
                Console.WriteLine(value_set[0].ToString());
                training_set.Rows.Add(value_set);
                               
            }

            set_view.DataSource = training_set;
            total = training_set.Rows.Count;
            txt_class.Text = total.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private string[] split_record(string record)
        {
            return (record.Split(' '));
        
        }
        
        private void writer(string file_nm)
        {
            try
            {

                TextWriter tw = new StreamWriter(file_nm);
                int len = output.Length;
                // write a line of text to the file
                for (int i = 0; i < index; i++)
                {
                    tw.WriteLine(output[i]);
                }
                tw.Close();
            }
            catch(Exception ex)
            {
            
            }
        }
        
        /// <summary>
        /// check if there is only one class in the trainee set.
        /// </summary>
        /// <returns></returns>
        private Boolean Initial_check(DataTable table)
        {
            bool flag = true;
            int total_row = table.Rows.Count;
            int total_col=table.Columns.Count;

            try
            {
                // Console.WriteLine("total col=" + total_col + " , total row= "+total_row);
                string temp = table.Rows[0]["result"].ToString();//error unknown "result" does not benlong to table
                //Console.WriteLine("temp="+temp);
                for (int l = 0; l < total_row; l++)
                {
                    if (temp != table.Rows[l]["result"].ToString())
                    {
                        flag = false;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
             flag=false;
            }
             return flag;
        }
        private int selected_attr(double[] gr,int no_of_index)
        {
            double max_g=gr[0];
            int max_index=0;
            if (no_of_index > 1)
            {
                for (int i = 1; i < no_of_index; i++)
                {
                    if (max_g < gr[i])
                    {
                        max_g = gr[i];
                        max_index = i;
                    }
                }
            }

            return max_index;
        
        }
        /// <summary>
        /// build sub table based on the current decision
        /// </summary>
        /// <returns></returns>
        private DataTable build_table(DataTable table, string col_name, string attr_value)
        {

            DataTable dt_temp = new DataTable();
            DataColumn col_temp = new DataColumn();
            col_temp = dt_temp.Columns.Add(col_name);
            col_temp.DataType=table.Columns[col_name].DataType;
            col_temp = dt_temp.Columns.Add("result");
            col_temp.DataType = table.Columns["result"].DataType;
            for (int i = 0; i < table.Rows.Count;i++ )
            {
                if (attr_value == table.Rows[i][col_name].ToString())
                {
                    dt_temp.Rows.Add (new object[] {table.Rows[i][col_name],table.Rows[i]["result"]});
                }
            }
            return dt_temp;
        }
        /// <summary>
        /// calculate information carried
        /// </summary>
        /// <returns></returns>
        private double info(int n, int[] freq,int total_row)
        {

            double temp,sum=0;

            for(int i=0 ; i<n ; i++)
            {
                temp = (double)freq[i] / total_row;
                sum -= temp * Math.Log(temp, 2);

            }
           
            return sum;

        }
        /// <summary>
        /// calcualte atrribute specific information
        /// dt= inpur table
        /// attr= attribute
        /// info_sum=information carried by class_attribute
        /// </summary>
        /// <returns></returns>
        private double info_x(DataTable dt, string attr,double info_sum,int unkwn_no)
        {
            double temp,temp_splt, sum_gain = 0,sum_split=0 ;
            int temp_total=dt.Rows.Count;
            int [] temp_cl=count_freq(dt,temp_total,attr);
            //Console.WriteLine(temp_cl[0] + " " + temp_cl[1] + " " + temp_cl[2]);
            string[] temp_attr = class_list;
            int n = class_no;
            for (int i = 0; i < n; i++)
            {
                if (temp_attr[i] != "?")
                {
                    temp = (double)temp_cl[i] / (temp_total-unkwn_no);//// temp_total=temp_total-missing_value
                    DataTable temp_dt = build_table(dt, attr, temp_attr[i]);
                    int no_row = temp_dt.Rows.Count;
                    int[] temp_freq = count_freq(temp_dt, no_row, "result");
                    double temp_sum = info(class_no, temp_freq, no_row);
                    sum_gain += (temp * temp_sum);
                }
                temp_splt = (double)temp_cl[i] / temp_total;
                sum_split -= (temp_splt * Math.Log(temp_splt, 2));
                
              
            }
            //Console.WriteLine("SUM= " + sum_gain);
            double f= (temp_total-unkwn_no)/temp_total;
            double gain = f*(info_sum - sum_gain);
            //Console.WriteLine("gain="+gain);
            double gain_ratio = gain / sum_split;
            //Console.WriteLine("gain ratio= "+gain_ratio);
            return gain_ratio;
        }
        /// <summary>
        /// calculate frequency
        /// </summary>
        /// <returns></returns>
        private int[] count_freq(DataTable class_col,int total_row,string col_nam)
        {
            //Console.WriteLine("count Freq entered");
            int k = 0;
            string[] clas = new string [10];//assume maximum no of class=10//for initialization
            //error prone
            //if (total_row == 1)///modification
            //{
              //  return new int[] { 0 };
            //}
            clas[0] = class_col.Rows[0][col_nam].ToString();
            for (int i = 1; i < total_row-1; i++ )
            {
                bool test = true;
                ////////////////////////////
                for( int l=0 ; l<=k ; l++)
                {
                    if (class_col.Rows[i][col_nam].ToString() == clas[l])
                    {
                        test = false;
                        break;
                    }
                }
                if (test != false)
                {
                    k++;
                    clas[k] = class_col.Rows[i][col_nam].ToString();
                }
            }
            ////////////////////////////////
            class_list = clas;
            //Console.WriteLine( "class value:"+class_list[k]);
            int clas_no = k + 1;
            class_no = clas_no;//number of class 
            int[] freq_result=new int[clas_no];
            ///
            ///calculating freq of each class at below
            ///
            for (int i = 0; i <clas_no; i++)
            {
                for (int j = 0; j < total_row; j++)
                {
                    if (clas[i] == class_col.Rows[j][col_nam].ToString())
                    {
                        freq_result[i] = freq_result[i] + 1;
                    }
                }
            }

            return freq_result;
        }
        private string[] item_list(DataTable class_col, int total_row, string col_nam)
        {
            int k = 0;
            string[] clas = new string[10];//assume maximum no of class=10//for initialization
            //if (class_col.Columns.Count == 0)//modification
            //{
            //    return new string { "" };
            //}
            clas[0] = class_col.Rows[0][col_nam].ToString();
            for (int i = 1; i < total_row - 1; i++)
            {
                bool test = true;
                ////////////////////////////
                for (int l = 0; l <= k; l++)
                {
                    if (class_col.Rows[i][col_nam].ToString() == clas[l])
                    {
                        test = false;
                        break;
                    }
                }
                if (test != false)
                {
                    k++;
                    clas[k] = class_col.Rows[i][col_nam].ToString();
                }
            }
            string[] value_list=new string[k+1];//modification
            for (int i = 0; i <= k; i++)
            {
                value_list[i] = clas[i];
            }
                return value_list;
        }
        private DataTable[] subtable(DataTable table,string sel_attr)
        { 
            string[] value_list = item_list(table, table.Rows.Count, sel_attr);

            
            int no_of_col = table.Columns.Count;
            int table_index = 0;
            int k=0;
            DataTable [] dt_temp = new DataTable[value_list.Length];
            DataColumn col_temp = new DataColumn();
            //Console.WriteLine(value_list.Length);
            object[] new_rec = new object[no_of_col - 1];
            int[] indices=new int[no_of_col-1];//indices of active colimn
            foreach (string item_value in value_list)
            {
                dt_temp[table_index] = new DataTable();
                for (int i = 0; i < no_of_col; i++) 
                {
                    if (sel_attr != table.Columns[i].ColumnName.ToString())
                    {
                        col_temp = dt_temp[table_index].Columns.Add(table.Columns[i].ColumnName);
                        col_temp.DataType = table.Columns[i].DataType;
                        indices[k]=i;
                        k++;
                    }
                  }
                ///
                /// adding new record to te subtable
                ///
                foreach (DataRow record in table.Rows)
                {
                    if (record[sel_attr].ToString() == item_value)
                    {
                        for (int j = 0; j < k; j++)
                        { 
                          new_rec[j]=record[indices[j]];
                          //Console.WriteLine(new_rec[j].ToString()+" "+indices[j]);
                        }
                            dt_temp[table_index].Rows.Add(new_rec);
                    
                    }
                
                }
                table_index++;
                k = 0;
            }

            return dt_temp;                  
        }
        
        private void print_result()
        {  output=new string[rule_no];
            string[] class_value = item_list(training_set,training_set.Rows.Count,"result");

            traverse(DecisionTree,class_value);
           
            writer(file_out);
        }
        
        private void traverse(TreeView tview, string[] class_value)
        {
            
            TreeNode temp = new TreeNode();
            for (int k = 0; k < tview.Nodes.Count; k++)
            {
               temp = tview.Nodes[k];
               string  str = temp.Text;
               for (int i = 0; i < temp.Nodes.Count; i++)
               visitChildNodes(temp.Nodes[i],str,class_value);
                  
            }
            
            
        
        }
        int index=0;
        private void visitChildNodes(TreeNode node,string str,string[] class_value)
        {
             str = str+")->"+node.Text;
             bool flag_str = false; 
             for (int j = 0; j < node.Nodes.Count; j++)
             { 
                 visitChildNodes(node.Nodes[j],str,class_value); 
             }
             for (int i = 0; i < class_value.Length; i++)
             {
                 if (node.Text.Contains(class_value[i]))
                 {
                     flag_str = true;
                     break;
                 }
             }
             if (flag_str)
             {
                 Console.WriteLine(str);
                 output[index] = str;
                 index++;
                 
             }

        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            file_name = @txt_file_name.Text+".txt";
            file_out="result/"+file_name+"";
            file_stream = new StreamReader(file_name);
            set_tableParam(param_file);
            create_table();
            load_sample();

            int discrete = sample_info.discrete;
            int numeric = sample_info.cont;
            //set_view.DataSource = get_subtable(training_set,"col0","rain");
            TreeNode root_node = new TreeNode("Root");
            DecisionTree.Nodes.Add(root_node);
            //double wgt = 0.0;
            //int mis_no = 0;
            mountTree(training_set, discrete, numeric, root_node,0,0);
            print_result();

           // ////////////to assign null to training set
           //training_set = new DataTable();
           ////DecisionTree = new TreeView();
           ////set_view.Refresh();
     
        }//end of button

        private void mountTree(DataTable table,int dis_attr,int cont_attr,TreeNode node,double wght,int mi_no )
        {
            double discrete_gain = 0;
            double numeric_gain = 0;
            int cont_index = 0;
            int value_index = 0;
            int max_discret = 0;
            float pivot_value = 0;////thresold
            //Console.WriteLine(dis_attr + " :" + cont_attr);

            //if (table.Columns.Count <=2)
            //{
            //    if (Initial_check(table))
            //    {

            //        double no = count_clas(table, wght, mi_no);
            //        Console.WriteLine("class_no: " + no);
            //        TreeNode child_node = new TreeNode(table.Rows[0]["result"].ToString() + ")" + no + ")");
            //        node.Nodes.Add(child_node);

            //    }
            //    else
            //    {
            //        TreeNode child_node = new TreeNode("not taken");
            //        node.Nodes.Add(child_node);
            //        Console.WriteLine("column<=2");
            //    }
            //    return;
            //}
            ////// else condition
            //else {
           
            if (Initial_check(table)||table.Columns.Count==1||table.Rows.Count==1 )
                {
                    //TreeNode child_node = new TreeNode(table.Rows[0]["result"].ToString());
                    //node.Nodes.Add(child_node);

                    double no = count_clas(table,wght,mi_no);
                    Console.WriteLine("class_no: "+no);
                    TreeNode child_node = new TreeNode(table.Rows[0]["result"].ToString() + ")" + no + ")");
                    node.Nodes.Add(child_node);

                return;
                }

                else
                {
                    if (dis_attr <= 0)
                    {
                        discrete_gain = -3;
                    }
                    else
                    {
                        max_discret = select_disc_attr(table, dis_attr);
                        discrete_gain = gain_ratio_x[max_discret];
                    }
                    //int max_discret = select_disc_attr(table,dis_attr);
                    if (cont_attr <= 0)
                    {
                        numeric_gain = -3;

                    }
                    else
                    {
                        double[] max_contin = select_cont_attr(table, cont_attr, dis_attr);
                        numeric_gain = max_contin[1];
                        cont_index = (int)max_contin[2];
                        value_index = (int)max_contin[3];
                        pivot_value = (float)max_contin[0];
                        Console.WriteLine(pivot_value);
                    }

                    if (discrete_gain > numeric_gain)
                    {

                        string sel_attr = table.Columns[max_discret].ColumnName.ToString();
                        int main_table_row = table.Rows.Count;
                        string[] value_list = item_list(table, table.Rows.Count, sel_attr);
                        ////find table for missing value
                        DataTable miss_table = get_missing_table(value_list,table,sel_attr);
                        int miss_no = miss_table.Rows.Count;
                        //set_view.DataSource = miss_table;
                        // nu of missing record
                        //set_view.DataSource = miss_table;
                        //foreach (string values in value_list) { Console.WriteLine(" values:"+values); }
                        foreach (string values in value_list)
                        {
                            
                            if(values!="?")
                            {
                                
                            DataTable new_table = get_subtable(table, sel_attr, values);
                            int attr_row = new_table.Rows.Count;//// count the no of values in the table 
                                /////appending here

                            DataTable tab = merge_table(new_table,miss_table);
                            //set_view.DataSource = tab;
                            ////// missing value weight calculation
                            
                            double weight_miss = (double)attr_row / (main_table_row - miss_no);
                                //////
                            Console.WriteLine("weight for"+values+" ="+weight_miss+" miss_no"+miss_no);
                            TreeNode child_node = new TreeNode( sel_attr + "(" + values+"(");
                            node.Nodes.Add(child_node);
                            mountTree(tab, dis_attr - 1, cont_attr, child_node,weight_miss,miss_no);///
                            }
                         }
                    }
                    else
                    {
                        try
                        {
                            string sel_attr = table.Columns[cont_index].ColumnName.ToString();
                            DataTable[] sub_tab = get_cont_subtable(table, sel_attr, pivot_value);////index find
                            for (int i = 0; i < 2; i++)
                            {
                                string node_name;
                                if (i == 0)
                                {
                                    node_name = "(" + sel_attr + "<=" + pivot_value;
                                }
                                else
                                {
                                    node_name = "(" + sel_attr + ">" + pivot_value;
                                }
                                TreeNode child_node = new TreeNode(node_name);
                                node.Nodes.Add(child_node);
                                if (sub_tab[i].Rows.Count != 0)
                                {
                                    double cont_wgt = wght;
                                    int cont_miss = mi_no;
                                    mountTree(sub_tab[i], dis_attr, cont_attr - 1, child_node, cont_wgt, cont_miss);
                                }
                                else
                                { 
                                ////// create a leaf of indecision
                                
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //  TreeNode child_node = new TreeNode(table.Rows[0]["result"].ToString());
                            //TreeNode child_node = new TreeNode("pending");
                            //node.Nodes.Add(child_node);
                            Console.WriteLine("mount calling mount");
                        }
                    }

                }//end of else
            //}//
            ////else condition

        }
        private double count_clas(DataTable table,double wght,int no_mis)
        {
            double temp_wht = wght;
            int no_wgt = no_mis;
            double   count_cl = 0;
            int total_row = table.Rows.Count-no_wgt;
            int total_col = table.Columns.Count;
            
            //if (no_mis > 0)
            
            try
            {
                // Console.WriteLine("total col=" + total_col + " , total row= "+total_row);
                string temp = table.Rows[0]["result"].ToString();//error unknown "result" does not benlong to table
                //Console.WriteLine("temp="+temp);
                for (int l = 0; l < total_row; l++)
                {
                    //if (temp == table.Rows[l]["result"].ToString())
                    //{
                    //    //flag = false;

                    //}
                    count_cl++;
                }
            }
            catch (Exception ex)
            {

            }
            double temp_wgt = (double)(count_cl + (temp_wht*no_wgt));
            //Console.WriteLine(" return value: "+temp_wgt+" miss:"+no_mis+"wgt:"+wght);
            return temp_wgt;

        }
        private DataTable merge_table(DataTable t1,DataTable t2)
        {
            DataTable t = t1;
            t.Merge(t2);
            //for (int i = 0; i < t2.Rows.Count;i++ )
            //{
                
            //}
            //set_view.DataSource=t;
            return t;

        }
        /// <summary>
        /// get table fro missing value
        /// </summary>
        /// <param name="value_list"></param>
        /// <param name="dt"></param>
        /// <param name="sel_attr"></param>
        /// <returns></returns>
        private DataTable get_missing_table(string[] value_list,DataTable dt,string sel_attr)
        {
            DataTable new_table = new DataTable();
            foreach (string values in value_list)
            {
                if (values == "?")
                {

                    new_table = get_subtable(dt, sel_attr, values);
                    //set_view.DataSource = new_table;    
                }
            }
            
            return new_table;
        }

        private int cal_missing_no(DataTable table,int colm_index)
        {
            int total_miss=0;
            for (int i = 0; i < table.Rows.Count;i++ )
            {
                if(table.Rows[i][colm_index].ToString()=="?")
                {
                    total_miss++;
                }
            }
            return total_miss;
        }
        public DataTable pure_table(DataTable table,int colm_indx)
        {
            //int total_miss = 0;
            DataTable table3 = table.Copy();
            for (int i = 0; i < table3.Rows.Count; i++)
            {
                if (table3.Rows[i][colm_indx].ToString() == "?")
                {
                    table3.Rows.RemoveAt(i);
                }
            }
            return table3;
            
        }
        private int  select_disc_attr(DataTable table, int discrete)
        {
            int temp_row = table.Rows.Count;
            int temp_col = discrete;
            //int misin_value=cal_missing_no(table,discrete);//cal function for calculating missing value
            //int[] freq = count_freq(table, temp_row, "result");
            //freq of diff type of class
            
            int freq_len = class_no;
            //double sum = info(class_no, freq, temp_row);

            for (int i = 0; i < discrete; i++)
            {
                string col_n = table.Columns[i].ColumnName.ToString();
                int misin_value = cal_missing_no(table, i);
                DataTable miss_tb=pure_table(table,i);////return table without missing value
                int[] freq = count_freq(miss_tb, miss_tb.Rows.Count, "result");
                double sum = info(freq_len, freq, temp_row-misin_value);
                gain_ratio_x[i] = info_x(table, col_n, sum,misin_value);//// to pass missing value

            }
            int max_discret = selected_attr(gain_ratio_x, discrete);

            return max_discret;
        }

        private double[] select_cont_attr(DataTable table,int numeric_attr,int discret_attr)
        {
            double[] pivot_val=new double[numeric_attr];
            //double[] pivot_val = new double[numeric_attr];
            double[]gain_ratio_cont=new double[numeric_attr];
            //double[] gain_ratio_cont = new double[100];
            double[] max_result = new double[40];
            int[] pivot_index=new int[numeric_attr];
            //int[] pivot_index = new int[100];


            for (int i = 0; i < numeric_attr; i++)
            {
                double[] con_result = cal_contin_gain(table, discret_attr + i); ////-----------
                gain_ratio_cont[i] = con_result[1]; /// gain
                pivot_val[i]=con_result[0];// pivot value
                pivot_index[i]= (int)con_result[2];// value index
            }


            int max_cont = selected_attr(gain_ratio_cont,numeric_attr);
            max_result[0]=pivot_val[max_cont];
            max_result[1] =gain_ratio_cont[max_cont];
            max_result[2] = max_cont + discret_attr;
            max_result[3]=(double)pivot_index[max_cont];
            return max_result;
        }
        /// <summary>
        /// to form discrete subtable removing unnecessary column and rows
        /// </summary>
        /// <param name="table"> input</param>
        /// <param name="sel_attr">selected attribute </param>
        /// <param name="values">different value from value_list</param>
        /// <returns></returns>
        /// 
        private DataTable get_subtable(DataTable table,string sel_attr,string values)
        {          
            DataTable table1=table.Copy();            
            int  k = 0;
            int total_col = table1.Columns.Count;
            int total_row = table1.Rows.Count;
            
            for (int i = 0; i < total_row; i++)
            {
                if (table1.Rows[i][sel_attr].ToString() != values)
                {
                    table1.Rows.RemoveAt(i);
                    total_row--;
                    i--;
                }
            }
            table1.Columns.Remove(sel_attr);
           
           
            return table1;
        }
        /// <summary>
        /// to get continuous subtable by removing unnecessary column and rows
        /// 
        /// </summary>
        /// <param name="table">parent table</param>
        /// <param name="sel_attr">the attribute on which we will split</param>
        /// <param name="value_index"></param>
        /// <returns>returnstwo data table</returns>
        /// ///modification
        ///
        /*
        private DataTable[] get_cont_subtable(DataTable table, string sel_attr, int value_index)
        {
            DataTable[] dt = new DataTable[2];
            dt[0] = new DataTable();
            dt[1] = new DataTable();
            DataColumn colmn = new DataColumn();
            int k = 0;
            int total_col = table.Columns.Count;
            int total_row = table.Rows.Count;
            int[] indices = new int[total_col - 1];
            object[] new_rec = new object[total_col - 1];

            // formartion of table column



            for (int i = 0; i < total_col; i++)
            {
                if (sel_attr != table.Columns[i].ColumnName.ToString())
                {
                    colmn = dt[0].Columns.Add(table.Columns[i].ColumnName);
                    colmn.DataType = table.Columns[i].DataType;
                    colmn = dt[1].Columns.Add(table.Columns[i].ColumnName);
                    colmn.DataType = table.Columns[i].DataType;
                    indices[k] = i;
                    k++;
                }
            }


            //adding record to the new table
            int new_col_no = k;
            for (int i = 0; i < value_index; i++)
            {
                //if (rec[sel_attr].ToString() == values)

                for (int j = 0; j < new_col_no; j++)
                {
                    new_rec[j] = table.Rows[i][indices[j]];
                    //Console.WriteLine(new_rec[j].ToString()+" "+indices[j]);
                }
                dt[0].Rows.Add(new_rec);
            }

            for (int i = value_index; i < total_row; i++)
            {
                //if (rec[sel_attr].ToString() == values)

                for (int j = 0; j < new_col_no; j++)
                {
                    new_rec[j] = table.Rows[i][indices[j]];
                    //Console.WriteLine(new_rec[j].ToString()+" "+indices[j]);
                }
                dt[1].Rows.Add(new_rec);
            }
            return dt;
        }
        */
        private DataTable[] get_cont_subtable(DataTable table,string sel_attr, float  pivot_value)
        {
            DataTable[] dt = new DataTable[2];
            dt[0] = new DataTable();
            dt[1] = new DataTable();
            DataColumn colmn = new DataColumn();
            int k = 0;
            int total_col = table.Columns.Count;
            int total_row=table.Rows.Count;
            int[] indices = new int[total_col - 1];
            object[] new_rec = new object[total_col - 1];

            // formartion of table column



            for (int i = 0; i < total_col; i++)
            {
                if (sel_attr != table.Columns[i].ColumnName.ToString())
                {
                    colmn = dt[0].Columns.Add(table.Columns[i].ColumnName);
                    colmn.DataType = table.Columns[i].DataType;
                    colmn = dt[1].Columns.Add(table.Columns[i].ColumnName);
                    colmn.DataType = table.Columns[i].DataType;
                    indices[k] = i;
                    k++;
                }
            }


            //adding record to the new table
            int new_col_no = k;
            for (int i = 0; i <total_row;i++ )
            {
                if ((float)table.Rows[i][sel_attr] <= pivot_value)
                {
                    for (int j = 0; j < new_col_no; j++)
                    {
                        new_rec[j] = table.Rows[i][indices[j]];
                        //Console.WriteLine(new_rec[j].ToString()+" "+indices[j]);
                    }
                    dt[0].Rows.Add(new_rec);
                }
                else
                {
                    for (int j = 0; j < new_col_no; j++)
                    {
                        new_rec[j] = table.Rows[i][indices[j]];
                        //Console.WriteLine(new_rec[j].ToString()+" "+indices[j]);
                    }
                    dt[1].Rows.Add(new_rec);

                }
             }

            //for (int i = value_index; i <total_row ; i++)
            //{
            //    //if (rec[sel_attr].ToString() == values)

            //    for (int j = 0; j < new_col_no; j++)
            //    {
            //        new_rec[j] = table.Rows[i][indices[j]];
            //        //Console.WriteLine(new_rec[j].ToString()+" "+indices[j]);
            //    }
            //    dt[1].Rows.Add(new_rec);
            //}
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="coln_index"></param>
        /// <returns></returns>
        /// 
        
        private double[] cal_contin_gain(DataTable table, int coln_index)
        {

            string column_name = table.Columns[coln_index].ColumnName.ToString();
            float[] pivot = new float[arr_size];//increase array size if necessary
            int[] pivot_index = new int[arr_size];//increase array size if necessary
            double[] cont_gain_ratio = new double[arr_size];/// increase array size if necessary
            double[] result = new double[3];//result[0]= pivot value and result[1]=gain ratio rasult[2]=value_index;
            int gain_len = 0;


            ////form continuous table with continous attribute
            DataTable temp_dt = cont_table(table,column_name);
            ///sort the continuous table
            DataTable sorted_tb = buble_sort(temp_dt,column_name);
            
            int total_row=sorted_tb.Rows.Count;
            string pivot_class = sorted_tb.Rows[0]["result"].ToString();// take initial class value
            float pivot_value=(float) sorted_tb.Rows[0][column_name];// take initial continous value
            // for any change in class value calculate gain 
            for (int i=1;i<total_row;i++)
            { 
                if(pivot_class!=sorted_tb.Rows[i]["result"].ToString())
                {
                    pivot_class = sorted_tb.Rows[i]["result"].ToString();
                    pivot_value = (float) sorted_tb.Rows[i][column_name];
                    pivot[gain_len] = pivot_value;
                    pivot_index[gain_len] = i;
                    cont_gain_ratio[gain_len]=cont_gain_x(sorted_tb,pivot_value,i-1); //// here wew are 
                    gain_len++;
                }
            }
            int k = selected_value(cont_gain_ratio,gain_len);
            result[0] = pivot[k]; //// pivot value
            result[1] = cont_gain_ratio[k];//gain
            result[2] =  pivot_index[k];//pivot index
            return result;

        }


        private int selected_value(double[] ratio,int arr_len)
        {
            double max_g = ratio[0];
            int max_index = 0;
            if (arr_len > 1)
            {
                for (int i = 1; i < arr_len; i++)
                {
                    if (max_g < ratio[i])
                    {
                        max_g = ratio[i];
                        max_index = i;
                    }
                }
            }

            return max_index;
        }
        /// <summary>
        /// //sort the table according to the given column ascending order
        /// </summary>
        /// <param name="table"></param>
        /// <param name="sort_column"></param>
        /// <returns></returns>
        /// 

        private DataTable buble_sort(DataTable table,string sort_column)
        
        {
            int n = table.Rows.Count;
            float temp_value;
            string temp_class;
             
            for (int i = 0; i<n; i++)
            {
                for (int j = 0; j <i;j++ )
                {

                    if ((float)table.Rows[i][sort_column] < (float)table.Rows[j][sort_column])
                    {
                        temp_value=(float)table.Rows[j][sort_column];
                        temp_class=table.Rows[j]["result"].ToString();

                        table.Rows[j][sort_column] = table.Rows[i][sort_column];
                        table.Rows[j]["result"] = table.Rows[i]["result"].ToString();
                        
                        table.Rows[i][sort_column] = temp_value;
                        table.Rows[i]["result"] = temp_class;
                        //Console.WriteLine("i am here");
                    }
                 }
             }
            //set_view.DataSource = table;
            Console.WriteLine("running..");
            
            return table;
            
        }

        /// <summary>
        ///calculate the gain of the value at which class value changes  
        /// </summary>
        /// <param name="table"> sorted table</param>
        /// <param name="pivot_value">value at which class value changes</param>
        /// <param name="pivot_index">index at which class value changes</param>
        /// <returns></returns>
        /// 

        private double cont_gain_x(DataTable table,float pivot_value,int pivot_index)
        {
            double gain=0;
            int main_total=table.Rows.Count;
            // gain of main table
            int[] freq_main = count_freq(table,main_total, "result");
            double main_sum = info(class_no, freq_main, main_total);
            //
            DataTable[] cont_tables = cont_sub_table(table,table.Columns[0].ColumnName.ToString(),pivot_index);
            //set_view.DataSource=cont_tables[0];
            double gain_sum = 0, split_sum = 0;
            for (int i = 0; i < 2; i++)
            { 
               //////////
                
                int total_row=cont_tables[i].Rows.Count;
                if (total_row != 0)
                {
                    int[] freq = count_freq(cont_tables[i], total_row, "result");
                    double temp_sum = info(class_no, freq, total_row);
                    double temp_value = (double)total_row / main_total;
                    gain_sum += temp_value * temp_sum;// calculate gain sum
                    split_sum -= split_value(main_total, total_row);
                }
                else 
                {
                    Console.WriteLine(" contin total row"+total_row);
                }
                //calculate splitsum             
            }
            gain = main_sum - gain_sum;
            double gain_ratio=0.0;
            
            gain_ratio = gain / split_sum;             
            
            return gain_ratio;
        }


        private double split_value(int gross_total,int sub_total)
        {
            double temp = (double)sub_total / gross_total;
            double split_x = temp * Math.Log(temp,2);
            return split_x;
           
        }
        /// <summary>
        /// uncessarily designed but can not delete untill sure
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cont_colmn"></param>
        /// <returns></returns>
        /// 

        private DataTable cont_table(DataTable table, string cont_colmn)
        {
            DataTable dt_temp = new DataTable();
            DataColumn col_temp = new DataColumn();
            col_temp = dt_temp.Columns.Add(cont_colmn);
            col_temp.DataType = table.Columns[cont_colmn].DataType;
            col_temp = dt_temp.Columns.Add("result");
            col_temp.DataType = table.Columns["result"].DataType;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                
                    dt_temp.Rows.Add(new object[] { table.Rows[i][cont_colmn], table.Rows[i]["result"] });
                
            }
            return dt_temp; 
        
        }

        /// <summary>
        /// binary splt of the dtata table base on te pivot value
        /// </summary>
        /// <param name="table"></param>
        /// <param name="col_name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// 
        private DataTable[] cont_sub_table(DataTable table, string col_name, int index)
        {
            DataTable[] dt_temp = new DataTable[2];

            DataColumn col_temp = new DataColumn();
            //form the table[0] column <= pivot value
            dt_temp[0] = new DataTable();
            col_temp = dt_temp[0].Columns.Add(col_name);
            col_temp.DataType = table.Columns[col_name].DataType;
            col_temp = dt_temp[0].Columns.Add("result");
            col_temp.DataType = table.Columns["result"].DataType;
            //form the table[0] column > pivot value
            dt_temp[1] = new DataTable();
            col_temp = dt_temp[1].Columns.Add(col_name);
            col_temp.DataType = table.Columns[col_name].DataType;
            col_temp = dt_temp[1].Columns.Add("result");
            col_temp.DataType = table.Columns["result"].DataType;

            for (int i = 0; i <= index; i++)
            {


                dt_temp[0].Rows.Add(new object[] { table.Rows[i][col_name], table.Rows[i]["result"] });

            }


            for (int i = index + 1; i < table.Rows.Count; i++)
            {


                dt_temp[1].Rows.Add(new object[] { table.Rows[i][col_name], table.Rows[i]["result"] });

            }

            return dt_temp;
        }

        private void txt_file_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void DecisionTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txt_class_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_cont_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_discrete_TextChanged(object sender, EventArgs e)
        {

        }
    }//end of class
}//end of namespace
