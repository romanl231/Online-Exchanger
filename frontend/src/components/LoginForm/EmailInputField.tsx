const EmailInputField = ({
  label,
  name,
  value,
  onChange,
  onBlur,
  error,
}: {
  label: string;
  name: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur: (e: React.FocusEvent<HTMLInputElement>) => void;
  error?: string | false;
}) => (
  <div className="flex relative flex-col gap-2.5 w-full">
    <div className="relative">
    <label htmlFor={name} className="text-sm text-gray-300 mb-1">
      {label}
    </label>
    <input
      id={name}
      name={name}
      type="email"
      value={value}
      onChange={onChange}
      onBlur={onBlur}
      className={`w-full rounded-3xl border bg-zinc-800 
      border-neutral-700 h-[41px] px-4 text-gray-200
      ${error ? "border-red-500" : "border-neutral-700"} 
      outline-none focus:border-purple-400`}
    />
    </div>
    {error && <p className="text-red-500 text-sm">{error}</p>}
  </div>
);

export default EmailInputField;
