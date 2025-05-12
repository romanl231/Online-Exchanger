import React, { useState } from "react";
import EmailInputField from "../LoginForm/EmailInputField";
import PasswordInput from "../LoginForm/PasswordInput";
import Divider from "../LoginForm/Divider";
import GoogleSignIn from "../LoginForm/GoogleSignIn";
import SignUpButton from "./SignUpButton";
import { Link } from "react-router-dom";
import Logo from "../LoginForm/Logo";


const RegistrationForm: React.FC = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
  return (
    <section className="flex justify-center items-center w-screen h-screen bg-[#121212]">
      <article className="flex flex-col items-center px-10 py-12 bg-[#1E1E1E] 
      rounded-[30px] w-[536px] max-md:w-[85%] max-sm:px-5 max-sm:w-[95%]">
        
        <Logo/>

        <form className="flex flex-col gap-12 w-full">
          <EmailInputField
           label="Your email" 
           value={email} 
           onChange={e => setEmail(e.target.value)} />

          <PasswordInput 
          label="Create a password" 
          value={password} 
          onChange={e => setPassword(e.target.value)} />

          <PasswordInput 
          label="Confirm your password" 
          value={confirmPassword} 
          onChange={e => setConfirmPassword(e.target.value)} />

          <div className="flex flex-col gap-5 items-center w-full">
            
            <SignUpButton 
            email={email} 
            password={password} 
            confirmPassword={confirmPassword}/>

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