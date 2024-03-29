﻿using ANCD.Domain.Entities.DomainEntities;
using ANCD.Domain.Entities.Enums;

namespace ANCD.Domain.Entities
{
    public class MedicalExam : Entity, IAggregateRoot
    {
        public DateTime Date { get; private set; }

        public Guid DoctorId { get; private set; }

        public Guid PatientId { get; private set; }

        public EStatus Status { get; private set; }

        public Doctor Doctor { get; private set; }

        public Patient Patient { get; private set; }

        private MedicalExam() : base()
        { }

        public MedicalExam(DateTime date, Doctor doctor, Patient patient) : base()
        {
            Date = date;
            DoctorId = doctor.Id;
            Doctor = doctor;
            PatientId = patient.Id;
            Patient = patient;
            Status = EStatus.Scheduled;
        }

        public bool IsAcomplished() => Status.Equals(EStatus.Accomplished);

        public void Accomplish() => Status = EStatus.Accomplished;

        public bool HasDateConflict(MedicalExam other)
        {
            var examEndDate = Date.AddMinutes(30);
            return Date.Equals(other.Date) || (other.Date > Date && other.Date < examEndDate);
        }

        public void SetDoctor(Doctor doctor) => Doctor = doctor;

        public void SetPatient(Patient patient) => Patient = patient;
    }
}
