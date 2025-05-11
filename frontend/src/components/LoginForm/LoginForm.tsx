import React, { useState } from "react";
import EmailInputField from "./EmailInputField";
import PasswordInput from "./PasswordInput";
import Divider from "./Divider";
import GoogleSignIn from "./GoogleSignIn";
import SignInButton from "./SignInButton";


const LoginForm: React.FC = () => {
    const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  return (
    <section className="flex justify-center items-center w-screen h-screen bg-neutral-900">
      <article className="flex flex-col items-center px-10 py-12 bg-stone-900 rounded-[30px] w-[536px] max-md:w-[90%] max-sm:px-5 max-sm:w-[95%]">
        <h1 className="mb-24 text-4xl text-purple-400">ExchangeMe</h1>

        <form className="flex flex-col gap-12 w-full">
          <EmailInputField label="Your email" value={email} onChange={e => setEmail(e.target.value)} />

          <PasswordInput value={password} onChange={e => setPassword(e.target.value)} />

          <div className="flex flex-col gap-5 items-center w-full">
            
            <SignInButton email={email} password={password}/>

            <div className="flex flex-col text-sm text-gray-200">
              <p>You don't have an account yet?</p>
              <button type="button" className="mx-auto text-purple-400 cursor-pointer">
                Create one
              </button>
            </div>

            <Divider />

            <button type="button" className="text-sm text-purple-400 cursor-pointer">
              Forgot your password?
            </button>

            <GoogleSignIn />
          </div>
        </form>
      </article>
    </section>
  );
};

export default LoginForm;
