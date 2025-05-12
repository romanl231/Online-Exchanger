import React, { useState } from "react";
import { useFormik } from "formik";
import { registerValidationSchema } from "../../utils/validationSchema.ts";
import EmailInputField from "../LoginForm/EmailInputField";
import PasswordInput from "../LoginForm/PasswordInput";
import Divider from "../LoginForm/Divider";
import GoogleSignIn from "../LoginForm/GoogleSignIn";
import SignUpButton from "./SignUpButton";
import { Link } from "react-router-dom";
import Logo from "../LoginForm/Logo";

const RegistrationForm: React.FC = () => {
    const formik = useFormik({
      initialValues: {
        email: "",
        password: "",
        confirmPassword: "",
      },
      validationSchema: registerValidationSchema,
      onSubmit: (values) => {
        console.log("Registered with:", values);
      },
    })
  return (
    <section className="flex justify-center items-center w-screen h-screen bg-[#121212]">
      <article className="flex flex-col items-center px-10 py-12 bg-[#1E1E1E] 
      rounded-[30px] w-[536px] max-md:w-[85%] max-sm:px-5 max-sm:w-[95%]">
        
        <Logo/>

        <form className="flex flex-col gap-12 w-full">
          <EmailInputField
           label="Your email" 
           value={formik.values.email}
           onChange={formik.handleChange} 
           onBlur={formik.handleBlur}
           name="email"
           error={formik.touched.email && formik.errors.email} 
           />

          <PasswordInput 
          label="Create a password" 
          value={formik.values.password} 
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          name="password"
          error={formik.touched.password && formik.errors.password} 
          />

          <PasswordInput 
          label="Confirm your password" 
          value={formik.values.confirmPassword} 
          onChange={formik.handleChange}
          onBlur={formik.handleBlur}
          name="confirmPassword"
          error={formik.touched.confirmPassword && formik.errors.confirmPassword} 
          />

          <div className="flex flex-col gap-5 items-center w-full">
            
            <SignUpButton 
            email={formik.values.email} 
            password={formik.values.password}
            confirmPassword={formik.values.confirmPassword}
            disabled={!(formik.isValid && formik.dirty)}
            />

            <div className="flex flex-col text-sm text-gray-200">
              <p>Already have an account?</p>
              <Link to="/login" className="mx-auto text-purple-400 cursor-pointer">
                Log in
              </Link>
            </div>

            <Divider />

            <GoogleSignIn />
          </div>
        </form>
      </article>
    </section>
  );
};

export default RegistrationForm;