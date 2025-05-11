const EmailInputField = ({
  label,
  value,
  onChange,
}: {
  label: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}) => (
  <div className="flex flex-col w-full">
    <label className="mb-2 text-sm text-gray-300">{label}</label>
    <input
      type="email"
      value={value}
      onChange={onChange}
      className="px-4 py-2 rounded-md bg-neutral-800 text-white"
    />
  </div>
);

export default EmailInputField;
