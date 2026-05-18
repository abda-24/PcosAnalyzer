using System;

namespace Domain.Entities
{
    public class ClinicalData
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public float BMI { get; set; }

        public int BloodGroup { get; set; }
        public int PulseRate { get; set; }
        public int RR { get; set; }
        public float Hb { get; set; }

        public int Cycle { get; set; }
        public int CycleLength { get; set; }

        public int MarriageStatus { get; set; }
        public bool Pregnant { get; set; }
        public int NoOfAbortions { get; set; }

        public float BetaHCG_I { get; set; }
        public float BetaHCG_II { get; set; }

        public float FSH { get; set; }
        public float LH { get; set; }
        public float FSH_LH { get; set; }

        public float Hip { get; set; }
        public float Waist { get; set; }
        public float WaistHipRatio { get; set; }

        public float TSH { get; set; }
        public float AMH { get; set; }
        public float PRL { get; set; }
        public float VitD3 { get; set; }
        public float PRG { get; set; }

        public float RBS { get; set; }

        public bool WeightGain { get; set; }
        public bool HairGrowth { get; set; }
        public bool SkinDarkening { get; set; }
        public bool HairLoss { get; set; }
        public bool Pimples { get; set; }

        public bool FastFood { get; set; }
        public bool RegExercise { get; set; }

        public int BP_Systolic { get; set; }
        public int BP_Diastolic { get; set; }

        public int FollicleNoL { get; set; }
        public int FollicleNoR { get; set; }

        public float AvgFSizeL { get; set; }
        public float AvgFSizeR { get; set; }

        public float Endometrium { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
