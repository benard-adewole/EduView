using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.Models
{
    public static class FacDeptImg
    {
        public static ObservableCollection<Model_Fac_dept> Data { get; set; }
        public static ObservableCollection<Model_Fac_dept> RetrieveData()
        {
            return new ObservableCollection<Model_Fac_dept>
            {
                new Model_Fac_dept{
                    Faculty= "Education",Image = "Edu.png",Selected = false, Departments =new ObservableCollection<DeptSelect>()
                    {
                        new DeptSelect{Department = "Department of Adult Education",Selected=false, Color = "#9F5AB7"},
                        new DeptSelect{Department = "Early Childhood Education",Selected=false, Color = "#BA5361"},
                        new DeptSelect{Department = "Education Management",Selected=false, Color ="#854732"},
                        new DeptSelect{Department = "Human Kinetics Education",Selected=false, Color = "#43716E"},
                        new DeptSelect{Department = "Arts and Social Science Education",Selected=false, Color =  "#DF6162"},
                        new DeptSelect{Department = "Science and Technology Education",Selected=false, Color = "#57AA76"},
                        
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Engineering",Image = "Engr.png",Selected = false,Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Biomedical",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "Civil",Selected =false, Color = "#BA5361" },
                        new DeptSelect{Department = "Computer",Selected =false, Color ="#854732" },
                        new DeptSelect{Department = "Electrical & electronics",Selected =false, Color = "#43716E" },
                        new DeptSelect{Department = "Mechanical",Selected =false, Color =  "#DF6162" },
                        new DeptSelect{Department = "Metallurgical & Materials",Selected =false, Color = "#57AA76" },
                        new DeptSelect{Department = "Systems",Selected =false, Color = "#40ADC4" },
                        new DeptSelect{Department = "Surveying and geoinformatics",Selected =false,Color = "#9F7E39" }
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Sciences",Image = "Science.png",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Biochemistry",Selected =false, Color = "#43716E" },
                        new DeptSelect{Department = "Botany",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "Cell Biology And Genetics",Selected =false,Color = "#BA5361" },
                        new DeptSelect{Department = "Chemistry",Selected =false ,Color ="#854732"},
                        new DeptSelect{Department = "Computer Sciences",Selected =false, Color =  "#DF6162" },
                        new DeptSelect{Department = "Geosciences",Selected =false , Color = "#57AA76" },
                        new DeptSelect{Department = "Mathematics",Selected =false, Color = "#40ADC4" },
                        new DeptSelect{Department = "Marine Science",Selected =false,Color = "#9F7E39" },
                        new DeptSelect{Department = "Microbiology",Selected =false, Color = "#43716E" },
                        new DeptSelect{Department = "Physics",Selected =false, Color = "#9F5AB7"},
                        new DeptSelect{Department = "Zoology",Selected =false,Color = "#BA5361" }
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Environmental Sciences",Image = "evnsci.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Department of Architecture",Selected =false, Color = "#43716E" },
                        new DeptSelect{Department = "Building",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "Estate Management",Selected =false,Color = "#BA5361" },
                        new DeptSelect{Department = "Quantity Surveying",Selected =false,Color ="#854732" },
                        new DeptSelect{Department = "Urban and Regional Planning",Selected =false , Color = "#40ADC4" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Law",Image = "law.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Law",Selected =false, Color =  "#DF6162" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Arts",Image = "art.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Linguistics",Selected =false, Color =  "#DF6162" },
                        new DeptSelect{Department = "Philosophy",Selected =false,Color = "#BA5361" },
                        new DeptSelect{Department = "Creative Arts",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "English",Selected =false, Color = "#40ADC4" },
                        new DeptSelect{Department = "History and Strategic Studies",Selected =false,Color ="#854732" },
                        new DeptSelect{Department = "European Language and Integration Studies",Selected =false, Color = "#43716E" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Social Sciences",Image = "fss.png",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Department of Economics",Selected =false, Color = "#57AA76" },
                        new DeptSelect{Department = "Geography",Selected =false, Color =  "#DF6162" },
                        new DeptSelect{Department = "Mass Communication",Selected =false,Color = "#BA5361" },
                        new DeptSelect{Department = "Political Science",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "Psychology",Selected =false, Color = "#40ADC4" },
                        new DeptSelect{Department = "Social Work",Selected =false ,Color ="#854732"},
                        new DeptSelect{Department = "Sociology",Selected =false, Color = "#43716E" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Basic Medical Sciences",Image = "medsci.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Anatomy",Selected =false, Color = "#57AA76" },
                        new DeptSelect{Department = "Anatomic and Molecular Pathology",Selected =false, Color =  "#DF6162" },
                        new DeptSelect{Department = "Biochemistry",Selected =false,Color = "#BA5361"  },
                        new DeptSelect{Department = "Medical Laboratory Science (MEDLAB)",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "Microbiology and Parasitology",Selected =false, Color = "#40ADC4" },
                        new DeptSelect{Department = "Pharmacology, Therapeutics and Toxicology",Selected =false ,Color ="#854732" },
                        new DeptSelect{Department = "Physiology",Selected =false, Color = "#43716E" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Management Science",Image = "mgmt.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Department of Accounting",Selected =false, Color = "#57AA76" },
                        new DeptSelect{Department = "Actuarial Science and Insurance",Selected =false, Color =  "#DF6162" },
                        new DeptSelect{Department = "Banking and Finance",Selected =false,Color = "#BA5361" },
                        new DeptSelect{Department = "Business Administration",Selected =false, Color = "#9F5AB7" },
                        new DeptSelect{Department = "Industrial Relations & Personnel Management",Selected =false,Color ="#854732" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Pharmacy",Image = "pharmacy.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Pharmacy",Selected =false, Color =  "#DF6162" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Clinical Science",Image = "CliSci.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Clinical Science",Selected =false, Color =  "#DF6162" },
                    }
                },
                new Model_Fac_dept{
                    Faculty= "Dental Science",Image = "dental.jpg",Selected = false, Departments =new ObservableCollection<DeptSelect>
                    {
                        new DeptSelect{Department = "Dental Science",Selected =false, Color =  "#DF6162" },
                    }
                },

            };
        }
    }
}
