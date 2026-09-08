using System;

namespace BaiTap1_BankAccount
{
    public class BankAccount
    {
        private static long _nextAccountNumber = 1000000001;

        private decimal _balance;

        public long AccountNumber { get; init; }

        private string _accountHolder;

        public string AccountHolder
        {
            get => _accountHolder;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        "Tên chủ tài khoản không được để trống."
                    );
                }

                _accountHolder = value;
            }
        }

        public decimal Balance
        {
            get => _balance;
            private set => _balance = value;
        }

        public BankAccount(string accountHolder, decimal initialBalance)
        {
            if (initialBalance < 50000)
            {
                throw new ArgumentException(
                    "Số dư ban đầu phải tối thiểu 50.000 VNĐ."
                );
            }

            AccountNumber = _nextAccountNumber++;
            AccountHolder = accountHolder;
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(
                    "Số tiền nạp phải lớn hơn 0."
                );
            }

            Balance += amount;

            Console.WriteLine(
                $"Nạp thành công {amount:N0} VNĐ."
            );
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine(
                    "Số tiền rút phải lớn hơn 0."
                );

                return false;
            }

            if (Balance - amount < 50000)
            {
                Console.WriteLine(
                    "Rút tiền thất bại! Số dư phải duy trì tối thiểu 50.000 VNĐ."
                );

                return false;
            }

            Balance -= amount;

            Console.WriteLine(
                $"Rút thành công {amount:N0} VNĐ."
            );

            return true;
        }

        public void DisplayInfo()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Số tài khoản: {AccountNumber}");
            Console.WriteLine($"Chủ tài khoản: {AccountHolder}");
            Console.WriteLine($"Số dư: {Balance:N0} VNĐ");
            Console.WriteLine("-----------------------------");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BankAccount account1 =
                    new BankAccount("Nguyễn Văn A", 1000000);

                BankAccount account2 =
                    new BankAccount("Trần Thị B", 500000);

                Console.WriteLine("=== TÀI KHOẢN BAN ĐẦU ===");

                account1.DisplayInfo();
                account2.DisplayInfo();

                Console.WriteLine("\n=== THAO TÁC TÀI KHOẢN 1 ===");

                account1.Deposit(200000);

                account1.Withdraw(300000);

                account1.Withdraw(900000);

                account1.DisplayInfo();

                Console.WriteLine(
                    "\n=== KIỂM TRA TÀI KHOẢN KHÔNG HỢP LỆ ==="
                );

                BankAccount account3 =
                    new BankAccount("Lê Văn C", 30000);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
    }
}