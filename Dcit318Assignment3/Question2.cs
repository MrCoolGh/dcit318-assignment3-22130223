using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcit318Assignment3
{
    // Q2(a): Generic repository
    public class Repository<T>
    {
        private readonly List<T> items = new();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            T? item = items.FirstOrDefault(predicate);
            if (item is null)
            {
                return false;
            }

            return items.Remove(item);
        }
    }

    // Q2(b): Patient
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"Patient {{ Id = {Id}, Name = {Name}, Age = {Age}, Gender = {Gender} }}";
        }
    }

    // Q2(c): Prescription
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString()
        {
            return $"Prescription {{ Id = {Id}, PatientId = {PatientId}, Medication = {MedicationName}, DateIssued = {DateIssued:d} }}";
        }
    }

    // Q2(g): Health system app
    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private readonly Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Alice Mensah", 28, "Female"));
            _patientRepo.Add(new Patient(2, "Kwame Asante", 35, "Male"));
            _patientRepo.Add(new Patient(3, "Esi Boateng", 22, "Female"));

            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Vitamin C", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Ibuprofen", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Paracetamol", DateTime.Now.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(5, 2, "Cough Syrup", DateTime.Now.AddDays(-7)));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap.Clear();

            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }

                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.TryGetValue(patientId, out var prescriptions)
                ? prescriptions
                : new List<Prescription>();
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("All Patients:");
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine(patient);
            }
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            var patient = _patientRepo.GetById(p => p.Id == id);
            if (patient is null)
            {
                Console.WriteLine($"No patient found with ID {id}.");
                return;
            }

            Console.WriteLine($"Prescriptions for {patient.Name} (ID: {id}):");
            var prescriptions = GetPrescriptionsByPatientId(id);

            if (prescriptions.Count == 0)
            {
                Console.WriteLine("No prescriptions found.");
                return;
            }

            foreach (var prescription in prescriptions)
            {
                Console.WriteLine(prescription);
            }
        }

        public void Run()
        {
            Console.WriteLine("=== QUESTION 2: Healthcare System ===");

            SeedData();
            BuildPrescriptionMap();
            PrintAllPatients();
            Console.WriteLine();

            int selectedPatientId = 1;
            PrintPrescriptionsForPatient(selectedPatientId);
            Console.WriteLine();
        }
    }
}
