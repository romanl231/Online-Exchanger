const EmailInputField = ({
  label,
  value,
  onChange,
}: {
  label: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}) => (
  <div className="flex relative flex-col gap-2.5 w-full">
    <div className="relative">
    <label className="text-sm text-gray-300 mb-1">{label}</label>
    <input
      type="email"
      value={value}
      onChange={onChange}
      className="w-full rounded-3xl border bg-zinc-800 
      border-neutral-700 h-[41px] px-4 text-gray-200 
      outline-none focus:border-purple-400"
    />
    </div>
  </div>
);

export default EmailInputField;
