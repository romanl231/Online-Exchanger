import React from "react";
import { Link } from "react-router-dom";
import { useFormik } from "formik";
import { loginValidationSchema } from "../../utils/validationSchema.ts";
import EmailInputField from "./EmailInputField";
import PasswordInput from "./PasswordInput";
import Divider from "./Divider";
import GoogleSignIn from "./GoogleSignIn";
import SignInButton from "./SignInButton";
import Logo from "../Logo.tsx";

const LoginForm: React.FC = () => {
  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
    },
    validationSchema: loginValidationSchema,
    onSubmit: (values) => {
      console.log("Logging in with:", values);
    },
  });

  return (
  <section className="flex justify-center items-center w-screen h-screen bg-[#121212]">
    <article className="flex flex-col items-center px-10 py-12 bg-[#1E1E1E] 
    rounded-[30px] w-[536px] max-md:w-[85%] max-sm:px-5 max-sm:w-[95%]">
      
      <Logo />

      <form className="w-full flex flex-col gap-5 items-center">
        <EmailInputField
          label="Your email"
          value={formik.values.email}
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          name="email"
          error={formik.touched.email && formik.errors.email}
           />

        <PasswordInput
          label="Your password"
          value={formik.values.password}
          name="password"
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          error={formik.touched.password && formik.errors.password} 
          />

        <SignInButton 
        email={formik.values.email} 
        password={formik.values.password}
        disabled={!(formik.isValid && formik.dirty)} 
        />

        <div className="flex flex-col text-sm text-gray-200 text-center">
          <p>You don't have an account yet?</p>
          <Link to="/register" className="text-purple-400 cursor-pointer">
            <p>Create one</p>
          </Link>
        </div>

        <Divider />

        <Link to="/" className="text-purple-400 cursor-pointer text-sm">
          <p>Forgot your password?</p>
        </Link>

        <GoogleSignIn />
      </form>
    </article>
  </section>
  );
};

export default LoginForm;
