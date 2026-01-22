using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem
{
    public class Item
    {
        private string _title;
        private string _publisher;
        private int _publicationYear;

        public string Title
        {
            get { return _title; }
            set 
            { 
                if(string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Title cannot be null or empty.");
                _title = value;
            }
        }

        public string Publisher
        {
            get => _publisher;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Publisher cannot be null or empty.");
                _publisher = value;
            }
        }

        public int PublicationYear
        {
            get => _publicationYear;
            set
            {
                if (value < 0)
                    throw new InvalidItemDataException("Publication year must be positive.");
                _publicationYear = value;
            }

        }

        //this is a virtual method for polymorphism
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Title: {Title}, Publisher: {Publisher}, Year: {PublicationYear}");
        }
    }

    public class Book : Item
    {
        private string _author;
        public string Author
        {
            get => _author;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidItemDataException("Author name cannot be empty.");
                _author = value;
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Book] Title: {Title}, Author: {Author}, Publisher: {Publisher}, Year: {PublicationYear}");

        }

    }

    public class Magazine : Item
    {
        private int _issueNumber;
        public int IssueNumber
        {
            get => _issueNumber;
            set
            {
                if (value <= 0)
                    throw new InvalidItemDataException("Issue number must be greater than zero.");
                _issueNumber = value;
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Magazine] Title: {Title}, Issue: {IssueNumber}, Publisher: {Publisher}, Year: {PublicationYear}");
        }

    }
}
