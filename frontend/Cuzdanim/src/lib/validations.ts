import { z } from "zod";

/**
 * Login form validation schema
 */
export const loginSchema = z.object({
  email: z
    .string()
    .min(1, "Email zorunludur")
    .email("Geçerli bir email adresi giriniz"),
  password: z.string().min(1, "Şifre zorunludur"),
});

export type LoginFormData = z.infer<typeof loginSchema>;

/**
 * Register form validation schema
 * Backend validator kurallarıyla %100 uyumlu
 */
export const registerSchema = z.object({
  email: z
    .string()
    .min(1, "Email zorunludur")
    .email("Geçerli bir email adresi giriniz")
    .max(256, "Email 256 karakterden uzun olamaz"),

  password: z
    .string()
    .min(8, "Şifre en az 8 karakter olmalıdır")
    .regex(/[A-Z]/, "Şifre en az bir büyük harf içermelidir")
    .regex(/[a-z]/, "Şifre en az bir küçük harf içermelidir")
    .regex(/[0-9]/, "Şifre en az bir rakam içermelidir")
    .regex(/[\W_]/, "Şifre en az bir özel karakter içermelidir"),

  firstName: z
    .string()
    .min(1, "Ad zorunludur")
    .max(100, "Ad 100 karakterden uzun olamaz"),

  lastName: z
    .string()
    .min(1, "Soyad zorunludur")
    .max(100, "Soyad 100 karakterden uzun olamaz"),

  phoneNumber: z
    .string()
    .regex(
      /^(\+90|0)?[0-9]{10}$/,
      "Geçerli bir Türkiye telefon numarası giriniz (örn: 5551234567)"
    )
    .optional()
    .or(z.literal("")),

  dateOfBirth: z
    .string()
    .optional()
    .refine(
      (val) => {
        if (!val) return true;
        const date = new Date(val);
        return date < new Date();
      },
      { message: "Doğum tarihi bugünden önce olmalıdır" }
    ),
});

export type RegisterFormData = z.infer<typeof registerSchema>;

/**
 * Account form validation schema
 */
export const accountSchema = z.object({
  name: z
    .string()
    .min(1, "Hesap adı zorunludur")
    .max(200, "Hesap adı 200 karakterden uzun olamaz"),

  type: z.number({
    message: "Hesap tipi seçiniz",
  }),

  initialBalance: z.number({
    message: "Geçerli bir tutar giriniz",
  }),

  currency: z.number({
    message: "Para birimi seçiniz",
  }),

  bankName: z
    .string()
    .max(200, "Banka adı 200 karakterden uzun olamaz")
    .optional()
    .or(z.literal("")),

  iban: z
    .string()
    .max(34, "IBAN 34 karakterden uzun olamaz")
    .optional()
    .or(z.literal("")),

  cardLastFourDigits: z
    .string()
    .max(4, "Son 4 hane 4 karakterden uzun olamaz")
    .optional()
    .or(z.literal("")),

  creditLimit: z
    .number()
    .positive("Kredi limiti pozitif olmalıdır")
    .optional()
    .nullable()
    .or(z.undefined()),
});

export type AccountFormData = z.infer<typeof accountSchema>;

/**
 * Update account validation schema
 */
export const updateAccountSchema = z.object({
  name: z
    .string()
    .min(1, "Hesap adı zorunludur")
    .max(200, "Hesap adı 200 karakterden uzun olamaz"),

  isActive: z.boolean(),

  includeInTotalBalance: z.boolean(),
});

export type UpdateAccountFormData = z.infer<typeof updateAccountSchema>;

/**
 * Transaction form validation schema
 * Backend CreateTransactionCommandValidator kurallarıyla uyumlu
 */
export const transactionSchema = z.object({
  type: z.number().min(1, "İşlem tipi seçiniz"),

  amount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Tutar pozitif olmalıdır"),

  accountId: z.string().min(1, "Hesap seçiniz"),

  categoryId: z.string().min(1, "Kategori seçiniz"),

  transactionDate: z.string().min(1, "İşlem tarihi seçiniz"),

  description: z.string().optional().or(z.literal("")),

  notes: z.string().optional().or(z.literal("")),
});

export type TransactionFormData = z.infer<typeof transactionSchema>;

/**
 * Update transaction validation schema
 */
export const updateTransactionSchema = z.object({
  amount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Tutar pozitif olmalıdır"),

  categoryId: z.string().min(1, "Kategori seçiniz"),

  transactionDate: z.string().min(1, "İşlem tarihi seçiniz"),

  description: z.string().optional().or(z.literal("")),

  notes: z.string().optional().or(z.literal("")),
});

export type UpdateTransactionFormData = z.infer<typeof updateTransactionSchema>;

/**
 * Budget form validation schema
 */
export const budgetSchema = z.object({
  name: z
    .string()
    .min(1, "Bütçe adı zorunludur")
    .max(200, "Bütçe adı 200 karakterden uzun olamaz"),

  categoryId: z.string().min(1, "Kategori seçiniz"),

  amount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Tutar pozitif olmalıdır"),

  startDate: z.string().min(1, "Başlangıç tarihi seçiniz"),

  endDate: z.string().min(1, "Bitiş tarihi seçiniz"),

  alertThresholdPercentage: z
    .number()
    .min(0, "Uyarı eşiği 0-100 arasında olmalıdır")
    .max(100, "Uyarı eşiği 0-100 arasında olmalıdır")
    .optional()
    .or(z.literal(80)), // Default 80
});

export type BudgetFormData = z.infer<typeof budgetSchema>;

/**
 * Update budget validation schema
 */
export const updateBudgetSchema = z.object({
  name: z
    .string()
    .min(1, "Bütçe adı zorunludur")
    .max(200, "Bütçe adı 200 karakterden uzun olamaz"),

  amount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Tutar pozitif olmalıdır"),

  startDate: z.string().min(1, "Başlangıç tarihi seçiniz"),

  endDate: z.string().min(1, "Bitiş tarihi seçiniz"),

  alertThresholdPercentage: z
    .number()
    .min(0, "Uyarı eşiği 0-100 arasında olmalıdır")
    .max(100, "Uyarı eşiği 0-100 arasında olmalıdır"),
});

export type UpdateBudgetFormData = z.infer<typeof updateBudgetSchema>;

/**
 * Goal form validation schema
 */
export const goalSchema = z.object({
  name: z
    .string()
    .min(1, "Hedef adı zorunludur")
    .max(200, "Hedef adı 200 karakterden uzun olamaz"),

  description: z
    .string()
    .max(1000, "Açıklama 1000 karakterden uzun olamaz")
    .optional()
    .or(z.literal("")),

  targetAmount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Hedef tutar pozitif olmalıdır"),

  targetDate: z.string().min(1, "Hedef tarihi seçiniz"),

  icon: z.string().optional().or(z.literal("")),
});

export type GoalFormData = z.infer<typeof goalSchema>;

/**
 * Update goal validation schema
 */
export const updateGoalSchema = z.object({
  name: z
    .string()
    .min(1, "Hedef adı zorunludur")
    .max(200, "Hedef adı 200 karakterden uzun olamaz"),

  description: z
    .string()
    .max(1000, "Açıklama 1000 karakterden uzun olamaz")
    .optional()
    .or(z.literal("")),

  targetAmount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Hedef tutar pozitif olmalıdır"),

  targetDate: z.string().min(1, "Hedef tarihi seçiniz"),

  icon: z.string().optional().or(z.literal("")),
});

export type UpdateGoalFormData = z.infer<typeof updateGoalSchema>;

/**
 * Add goal contribution validation schema
 */
export const addContributionSchema = z.object({
  amount: z
    .number({
      message: "Geçerli bir tutar giriniz",
    })
    .positive("Katkı tutarı pozitif olmalıdır"),

  note: z
    .string()
    .max(500, "Not 500 karakterden uzun olamaz")
    .optional()
    .or(z.literal("")),
});

export type AddContributionFormData = z.infer<typeof addContributionSchema>;
