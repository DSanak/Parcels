using System;
using System.Collections.Generic;
using System.Linq;

namespace Paczki
{
    enum ParcelStatus
    {​​
        None,
        State1,
        State2,
        State3
    }​​
    class Deliverer
    {​​
        private readonly ICollection<Parcel> _parcels = new List<Parcel>();
        public event Action<string, ParcelStatus> ParcelStatusChanged;
        public void ChangeParcelStatus(string trackNumber)
        {​​
            // None -> State1
            // Status1 -> Status2
            // Status2 -> Status3
            // Status3 -> Exception
            var parcel = _parcels.FirstOrDefault(x => x.TrackNumber == trackNumber);
            if (parcel is null)
            {​​
                throw new ArgumentException();
            }​​
            if (parcel.Status == ParcelStatus.State3)
                throw new ArgumentException();
            parcel.Status++;
            ParcelStatusChanged?.Invoke(parcel.TrackNumber, parcel.Status);
        }​​
        public void Add(Parcel parcel)
        {​​
            _parcels.Add(parcel);
            ChangeParcelStatus(parcel.TrackNumber);
        }​​
    }​​
    class Parcel
    {​​
        public string TrackNumber {​​ get; set; }​​
        public ParcelStatus Status {​​ get; set; }​​
        public Parcel(string trackNumber)
        {​​
            TrackNumber = trackNumber;
            Status = ParcelStatus.None;
        }​​
    }​​
    class Client
    {​​
        public string Name {​​ get; set; }​​
        public string TrackNumber {​​ get; }​​
        public Client(string name, string trackNumber)
        {​​
            Name = name;
            TrackNumber = trackNumber;
        }​​
        public void ParcelStatusSub(string trackNumber, ParcelStatus status)
        {​​
            if (trackNumber == TrackNumber)
            {​​
                Console.WriteLine("[{​​0}​​]: O moja paczka {​​1}​​ zmieniła status na: {​​2}​​", Name, trackNumber, status);
            }​​
        }​​
    }​​
    class Program
    {​​
        static void Main(string[] args)
        {​​
            // System śledzenia przesyłek
            // --------------------------
            // Dostawca
            //  - Zmiana statusu paczki
            // Paczka
            //  - Track Number
            //  - status: State_1, State_2, State_3
            // Klient
            //  - Zmiana statusu jego paczki
            var p1 = new Parcel("123456789ATH");
            var p2 = new Parcel("ATH123456789");
            var d = new Deliverer();
            var c1 = new Client("Janek", "ATH123456789");
            var c2 = new Client("Zosia", "123456789ATH");
            d.ParcelStatusChanged += c1.ParcelStatusSub;
            d.ParcelStatusChanged += c2.ParcelStatusSub;
            d.Add(p1);
            d.Add(p2);
            // --- symulacja
            d.ChangeParcelStatus("123456789ATH");
            d.ChangeParcelStatus("ATH123456789");
            d.ChangeParcelStatus("123456789ATH");
            d.ChangeParcelStatus("ATH123456789");
        }​​
    }​​
    }
