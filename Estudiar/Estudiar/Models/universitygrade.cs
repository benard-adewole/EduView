using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Estudiar.Models
{
    public class AllSavedData
    {
        public ObservableCollection<SemesterGroup> SemesterGroup { get; set; }
    }
    public class SemesterGroup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ObservableCollection<universitygrade> universitygrades { get; set;}
    }
    public class universitygrade
    {
        public int semester { get; set; }
        public int totalunits { get; set; } = 0;
        public float scoreobtained { get; set; } = 0f;
        public float overallobtainablescore { get; set; } = 0f;
        public float gpa { get; set; } = 0f;

       
        public ObservableCollection<CourseDetails> GetCourseDetails { get; set; } =    new ObservableCollection<CourseDetails>();
        public universitygrade()
        {
        }
        private List<int> possiblegradesvalues { get; set; } = new List<int>() { 5,4,3,2,1,0 };
        public void calculateGPA()
        {
            totalunits = 0;
            scoreobtained = 0f;
            overallobtainablescore = 0f;
            gpa = 0f;
            for (int i = 0; i < GetCourseDetails.Count; i++)
            {
                totalunits += GetCourseDetails[i].unit;
                scoreobtained +=  possiblegradesvalues[GetCourseDetails[i].gradeIndex] * GetCourseDetails[i].unit;
                overallobtainablescore += 5 * GetCourseDetails[i].unit;
            }

            if (overallobtainablescore == 0)
            {
                gpa = 0f;
            }
            else
            {
                gpa = scoreobtained * 5 / overallobtainablescore;
            }
            

        }
    }
}
