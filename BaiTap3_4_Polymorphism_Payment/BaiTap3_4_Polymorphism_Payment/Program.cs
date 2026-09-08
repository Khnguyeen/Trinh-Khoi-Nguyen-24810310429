using System;
using System.Collections.Generic;

namespace BaiTap3_4_Polymorphism_Payment
{
    public class DiscountCalculator
    {
        public decimal ApplyDiscount(decimal totalAmount)
        {
            return totalAmount * 0.95m;
        }

        public decimal ApplyDiscount(decimal totalAmount, double percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentException("Phần trăm giảm giá phải từ 0 đến 100.");
            }

            return totalAmount * (1 - (decimal)percentage / 100);
        }

        public decimal ApplyDiscount(decimal totalAmount, decimal fixedVoucher, decimal minimumOrder)
        {
            if (totalAmount >= minimumOrder)
            {
                decimal result = totalAmount - fixedVoucher;
                return result < 0 ? 0 : result;
            }

            return totalAmount;
        }
    }

    public class DeliveryService
    {
        public string OrderId { get; set; }
        public double DistanceKm { get; set; }

        public DeliveryService(string orderId, double distanceKm)
        {
            OrderId = orderId;
            DistanceKm = distanceKm;
        }

        public virtual decimal CalculateShippingFee()
        {
            return (decimal)DistanceKm * 5000;
        }
    }

    public class ExpressDelivery : DeliveryService
    {
        public ExpressDelivery(string orderId, double distanceKm)
            : base(orderId, distanceKm)
        {
        }

        public override decimal CalculateShippingFee()
        {
            return base.CalculateShippingFee() * 1.5m + 20000;
        }
    }

    public class EcoDelivery : DeliveryService
    {
        public EcoDelivery(string orderId, double distanceKm)
            : base(orderId, distanceKm)
        {
        }

        public override decimal CalculateShippingFee()
        {
            decimal basicFee = base.CalculateShippingFee();

            if (DistanceKm > 10)
            {
                return basicFee * 0.9m;
            }

            return basicFee;
        }
    }

    public interface IPayable
    {
        bool ProcessPayment(decimal amount);
    }

    public interface IRefundable
    {
        bool ProcessRefund(decimal amount, string reason);
    }

    public abstract class PaymentGateway
    {
        public string TransactionId { get; init; }
        public DateTime CreationDate { get; init; }
        public string Status { get; protected set; }

        protected PaymentGateway(string transactionId)
        {
            TransactionId = transactionId;
            CreationDate = DateTime.Now;
            Status = "Pending";
        }

        public abstract void ValidateConnection();

        public virtual void LogTransaction(string message)
        {
            Console.WriteLine($"[{TransactionId}] {message}");
        }
    }

    public class MomoPayment : PaymentGateway, IPayable, IRefundable
    {
        public string PhoneNumber { get; set; }

        public MomoPayment(string transactionId, string phoneNumber)
            : base(transactionId)
        {
            PhoneNumber = phoneNumber;
        }

        public override void ValidateConnection()
        {
            Console.WriteLine("Đang kiểm tra kết nối API MoMo...");
        }

        public bool ProcessPayment(decimal amount)
        {
            if (amount <= 0)
            {
                LogTransaction("Thanh toán thất bại: số tiền không hợp lệ.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                LogTransaction("Thanh toán thất bại: số điện thoại không hợp lệ.");
                return false;
            }

            Status = "Success";
            LogTransaction($"Thanh toán thành công {amount:N0} VNĐ.");

            return true;
        }

        public bool ProcessRefund(decimal amount, string reason)
        {
            if (amount <= 0)
            {
                LogTransaction("Hoàn tiền thất bại: số tiền không hợp lệ.");
                return false;
            }

            Status = "Refunded";
            LogTransaction($"Hoàn tiền {amount:N0} VNĐ. Lý do: {reason}");

            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== BÀI TẬP 3 =====");

            DiscountCalculator calculator = new DiscountCalculator();

            decimal total = 1000000;

            Console.WriteLine($"Đơn hàng: {total:N0} VNĐ");
            Console.WriteLine($"Giảm 5%: {calculator.ApplyDiscount(total):N0} VNĐ");
            Console.WriteLine($"Giảm 20%: {calculator.ApplyDiscount(total, 20):N0} VNĐ");
            Console.WriteLine($"Voucher 100.000: {calculator.ApplyDiscount(total, 100000, 500000):N0} VNĐ");

            List<DeliveryService> deliveries = new List<DeliveryService>
            {
                new ExpressDelivery("DH01", 5),
                new EcoDelivery("DH02", 15),
                new DeliveryService("DH03", 8)
            };

            Console.WriteLine("\n===== PHÍ VẬN CHUYỂN =====");

            foreach (DeliveryService delivery in deliveries)
            {
                Console.WriteLine($"Mã đơn: {delivery.OrderId}");
                Console.WriteLine($"Khoảng cách: {delivery.DistanceKm} km");
                Console.WriteLine($"Phí vận chuyển: {delivery.CalculateShippingFee():N0} VNĐ");
                Console.WriteLine();
            }

            Console.WriteLine("===== BÀI TẬP 4 =====");

            MomoPayment momo = new MomoPayment(
                "TXN001",
                "0912345678"
            );

            momo.ValidateConnection();

            IPayable payment = momo;
            payment.ProcessPayment(500000);

            IRefundable refund = momo;
            refund.ProcessRefund(200000, "Khách hàng hủy đơn");

            Console.WriteLine("\n===== THÔNG TIN GIAO DỊCH =====");
            Console.WriteLine($"Mã giao dịch: {momo.TransactionId}");
            Console.WriteLine($"Ngày tạo: {momo.CreationDate}");
            Console.WriteLine($"Trạng thái: {momo.Status}");
        }
    }
}