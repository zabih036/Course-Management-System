using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace JahanInstitute
{
    public partial class JahanInstituteContext : DbContext
    {
        public IConfiguration configuration { get; set; }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<AttendanceSheet> AttendanceSheet { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<ClassName> ClassName { get; set; }
        public virtual DbSet<ClassTeacher> ClassTeacher { get; set; }
        public virtual DbSet<ClassTime> ClassTime { get; set; }
        public virtual DbSet<ClassType> ClassType { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<Dealing> Dealing { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeType> EmployeeType { get; set; }
        public virtual DbSet<ExamMarks> ExamMarks { get; set; }
        public virtual DbSet<ExamType> ExamType { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<HourlyClass> HourlyClass { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
        public virtual DbSet<SalaryType> SalaryType { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentClass> StudentClass { get; set; }
        public virtual DbSet<TeacherAttendance> TeacherAttendance { get; set; }
        public virtual DbSet<TeacherType> TeacherType { get; set; }
        public virtual DbSet<Time> Time { get; set; }
        public virtual DbSet<UserLoginDetail> UserLoginDetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

         
                var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                configuration = builder.Build();
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.AttendenceId);

                entity.Property(e => e.AttendenceId).HasColumnName("Attendence_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Attendance_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Attendance_Student");
            });

            modelBuilder.Entity<AttendanceSheet>(entity =>
            {
                entity.HasKey(e => e.SheetId);

                entity.ToTable("Attendance_Sheet");

                entity.Property(e => e.SheetId).HasColumnName("Sheet_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AttendanceSheet)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Attendance_Sheet_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.AttendanceSheet)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Attendance_Sheet_Student");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.ClassNameId).HasColumnName("ClassName_Id");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EndMiridiam).HasMaxLength(5);

                entity.Property(e => e.Shift).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StartMiridiam).HasMaxLength(5);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.ClassName)
                    .WithMany(p => p.Class)
                    .HasForeignKey(d => d.ClassNameId)
                    .HasConstraintName("FK_Class_ClassName");
            });

            modelBuilder.Entity<ClassName>(entity =>
            {
                entity.Property(e => e.ClassNameId).HasColumnName("ClassName_Id");

                entity.Property(e => e.ClassName1)
                    .HasColumnName("ClassName")
                    .HasMaxLength(50);

                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.ClassName)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_ClassName_Department");
            });

            modelBuilder.Entity<ClassTeacher>(entity =>
            {
                entity.ToTable("Class_Teacher");

                entity.Property(e => e.ClassTeacherId).HasColumnName("Class_Teacher_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Class_Teacher_Class");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.ClassTeacher)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Class_Teacher_Employee");
            });

            modelBuilder.Entity<ClassTime>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Class_Time");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.TimeId).HasColumnName("Time_Id");

                entity.HasOne(d => d.Class)
                    .WithMany()
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Class_Time_Class");

                entity.HasOne(d => d.Time)
                    .WithMany()
                    .HasForeignKey(d => d.TimeId)
                    .HasConstraintName("FK_Class_Time_Time");
            });

            modelBuilder.Entity<ClassType>(entity =>
            {
                entity.Property(e => e.ClassTypeId).HasColumnName("ClassType_Id");

                entity.Property(e => e.ClassType1)
                    .HasColumnName("ClassType")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.CurrencyId).HasColumnName("Currency_Id");

                entity.Property(e => e.Currency1)
                    .HasColumnName("Currency")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Dealer>(entity =>
            {
                entity.Property(e => e.DealerId).HasColumnName("Dealer_Id");

                entity.Property(e => e.Dealer1)
                    .HasColumnName("Dealer")
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(20);
            });

            modelBuilder.Entity<Dealing>(entity =>
            {
                entity.Property(e => e.DealingId).HasColumnName("Dealing_Id");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.CurrencyId).HasColumnName("Currency_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DealerId).HasColumnName("Dealer_Id");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Dealing)
                    .HasForeignKey(d => d.CurrencyId)
                    .HasConstraintName("FK_Dealing_Currency");

                entity.HasOne(d => d.Dealer)
                    .WithMany(p => p.Dealing)
                    .HasForeignKey(d => d.DealerId)
                    .HasConstraintName("FK_Dealing_Dealer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Dealing)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Dealing_Employee");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");

                entity.Property(e => e.Department1)
                    .HasColumnName("Department")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.EmployeeTypeId).HasColumnName("EmployeeType_Id");

                entity.Property(e => e.FatherName)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.FireDate).HasColumnType("date");

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.IdCardNo).HasColumnName("ID_Card_No");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Photo).HasMaxLength(256);

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.SalaryType).HasMaxLength(20);

                entity.Property(e => e.Village).HasMaxLength(50);

                entity.HasOne(d => d.EmployeeType)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.EmployeeTypeId)
                    .HasConstraintName("FK_Employee_EmployeeType");
            });

            modelBuilder.Entity<EmployeeType>(entity =>
            {
                entity.Property(e => e.EmployeeTypeId).HasColumnName("EmployeeType_Id");

                entity.Property(e => e.EmployeeId).HasMaxLength(50);
            });

            modelBuilder.Entity<ExamMarks>(entity =>
            {
                entity.HasKey(e => e.ExamId);

                entity.ToTable("Exam_Marks");

                entity.Property(e => e.ExamId).HasColumnName("Exam_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ExamTypeId).HasColumnName("ExamType_Id");

                entity.Property(e => e.HelpMarks).HasColumnName("Help_Marks");

                entity.Property(e => e.ObtainMarks).HasColumnName("Obtain_Marks");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.Property(e => e.TotalMarks).HasColumnName("Total_Marks");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ExamMarks)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Exam_Marks_Class");

                entity.HasOne(d => d.ExamType)
                    .WithMany(p => p.ExamMarks)
                    .HasForeignKey(d => d.ExamTypeId)
                    .HasConstraintName("FK_Exam_Marks_ExamType");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ExamMarks)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Exam_Marks_Student");
            });

            modelBuilder.Entity<ExamType>(entity =>
            {
                entity.Property(e => e.ExamTypeId).HasColumnName("ExamType_Id");

                entity.Property(e => e.ExamType1)
                    .HasColumnName("ExamType")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.ExpenseId).HasColumnName("Expense_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ExpenseTypeId).HasColumnName("ExpenseType_Id");

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.Expense)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .HasConstraintName("FK_Expense_ExpenseType");
            });

            modelBuilder.Entity<ExpenseType>(entity =>
            {
                entity.Property(e => e.ExpenseTypeId).HasColumnName("ExpenseType_Id");

                entity.Property(e => e.ExpenseType1)
                    .HasColumnName("ExpenseType")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Fees>(entity =>
            {
                entity.Property(e => e.FeesId).HasColumnName("Fees_Id");

                entity.Property(e => e.BillNo).HasColumnName("Bill_No");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Fees_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Fees_Student");
            });

            modelBuilder.Entity<HourlyClass>(entity =>
            {
                entity.HasKey(e => e.HourlyTimeId);

                entity.Property(e => e.HourlyTimeId).HasColumnName("HourlyTime_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.TotalHours).HasColumnName("Total_Hours");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.HourlyClass)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_HourlyClass_Class");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.HourlyClass)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_HourlyClass_Employee");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.NoteId).HasColumnName("Note_Id");

                entity.Property(e => e.Note1).HasColumnName("Note");

                entity.Property(e => e.NoteStatus).HasMaxLength(10);

                entity.Property(e => e.RemainDate).HasColumnType("datetime");

                entity.Property(e => e.TargetDate).HasColumnType("datetime");

                entity.Property(e => e.UserEmail).HasMaxLength(256);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.Property(e => e.SalaryId).HasColumnName("Salary_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.PaidAmount).HasColumnName("Paid_Amount");

                entity.Property(e => e.PaidBy).HasColumnName("Paid_By");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.SalaryNavigation)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Salary_Employee");
            });

            modelBuilder.Entity<SalaryType>(entity =>
            {
                entity.Property(e => e.SalaryTypeId).HasColumnName("SalaryType_Id");

                entity.Property(e => e.SalaryType1)
                    .HasColumnName("SalaryType")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FatherName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(20);

                entity.Property(e => e.IdCardNo).HasColumnName("ID_Card_No");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Photo).HasMaxLength(256);

                entity.Property(e => e.RegDate)
                    .HasColumnName("Reg_Date")
                    .HasColumnType("date");

                entity.Property(e => e.School).HasMaxLength(50);
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.HasKey(e => e.StdClassId);

                entity.ToTable("Student_Class");

                entity.Property(e => e.StdClassId).HasColumnName("StdClass_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.StudentClass)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Student_Class_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentClass)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Student_Class_Student");
            });

            modelBuilder.Entity<TeacherAttendance>(entity =>
            {
                entity.HasKey(e => e.SheetId);

                entity.ToTable("Teacher_Attendance");

                entity.Property(e => e.SheetId).HasColumnName("Sheet_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.Status).HasMaxLength(10);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TeacherAttendance)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Teacher_Attendance_Employee");
            });

            modelBuilder.Entity<TeacherType>(entity =>
            {
                entity.Property(e => e.TeacherTypeId).HasColumnName("TeacherType_Id");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TeacherType)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_TeacherType_Employee");
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.Property(e => e.TimeId).HasColumnName("Time_Id");
            });

            modelBuilder.Entity<UserLoginDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId);

                entity.Property(e => e.DetailId).HasColumnName("Detail_Id");

                entity.Property(e => e.EmployeeEmail).HasMaxLength(450);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
