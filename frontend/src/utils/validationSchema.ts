import * as Yup from "yup";

export const loginValidationSchema = Yup.object({
  email: Yup.string()
  .email("Invalid email")
  .required("Email is required"),

  password: Yup.string()
    .min(8, "Password must be at least 8 characters")
    .matches(/[a-z]/, "Must contain at least one lowercase letter")
    .matches(/[A-Z]/, "Must contain at least one uppercase letter")
    .matches(/\d/, "Must contain at least one number")
    .matches(/[@$!%*?&#^()_\-+=]/, "Must contain at least one special character")
    .required("Password is required")
});

export const registerValidationSchema = Yup.object({
    email: Yup.string()
    .email("Invalid email")
    .required("Email is required"),

    password: Yup.string()
    .min(8, "Password must be at least 8 characters")
    .matches(/[a-z]/, "Must contain at least one lowercase letter")
    .matches(/[A-Z]/, "Must contain at least one uppercase letter")
    .matches(/\d/, "Must contain at least one number")
    .matches(/[@$!%*?&#^()_\-+=]/, "Must contain at least one special character")
    .required("Password is required"),

    confirmPassword: Yup.string()
        .oneOf([Yup.ref("password")], "Passwords must match")
        .required("Please confirm your password"),
});