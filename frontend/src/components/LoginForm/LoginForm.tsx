import React, { useState } from "react";
import { Link } from "react-router-dom";
import EmailInputField from "./EmailInputField";
import PasswordInput from "./PasswordInput";
import Divider from "./Divider";
import GoogleSignIn from "./GoogleSignIn";
import SignInButton from "./SignInButton";
import Logo from "./Logo";

const LoginForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  return (
    <section className="flex justify-center items-center w-screen h-screen bg-[#121212]">

      <article className="flex flex-col items-center px-10 py-12 bg-[#1E1E1E] 
      rounded-[30px] w-[536px] max-md:w-[85%] max-sm:px-5 max-sm:w-[95%]">
        <Logo />

        <form className="text-red-500 text-4xl font-bold">
          <EmailInputField
            label="Your email"
            value={email}
            onChange={e => setEmail(e.target.value)} />

          <PasswordInput
            label="Your password"
            value={password}
            onChange={e => setPassword(e.target.value)} />

          <div className="flex flex-col gap-5 items-center w-full">

            <SignInButton email={email} password={password} />

            <div className="flex flex-col text-sm text-gray-200">
              <p>You don't have an account yet?</p>
              <Link to="/register" className="mx-auto text-purple-400 cursor-pointer">
                Create one
              </Link>
            </div>

            <Divider />

            <Link to="/" className="mx-auto text-purple-400 cursor-pointer">
              Forgot your password?
            </Link>

            <GoogleSignIn />
          </div>
        </form>
      </article>
    </section>
  );
};

export default LoginForm;
