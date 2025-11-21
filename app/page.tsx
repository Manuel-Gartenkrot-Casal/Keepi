export default function Page() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-green-50 to-blue-50 p-8">
      <div className="max-w-4xl mx-auto">
        <div className="bg-white rounded-lg shadow-lg p-8 mb-6">
          <div className="flex items-center gap-4 mb-6">
            <div className="w-16 h-16 bg-green-500 rounded-lg flex items-center justify-center text-white text-2xl font-bold">
              K
            </div>
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Keepi - C# ASP.NET MVC App</h1>
              <p className="text-gray-600">Smart Refrigerator Management System</p>
            </div>
          </div>

          <div className="bg-yellow-50 border-l-4 border-yellow-400 p-4 mb-6">
            <div className="flex items-start">
              <div className="flex-shrink-0">
                <svg className="h-5 w-5 text-yellow-400" viewBox="0 0 20 20" fill="currentColor">
                  <path
                    fillRule="evenodd"
                    d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z"
                    clipRule="evenodd"
                  />
                </svg>
              </div>
              <div className="ml-3">
                <h3 className="text-sm font-medium text-yellow-800">Important Notice</h3>
                <p className="mt-2 text-sm text-yellow-700">
                  This is a <strong>C# ASP.NET MVC</strong> application, not a Next.js/React app. To run this project,
                  you need Visual Studio and SQL Server.
                </p>
              </div>
            </div>
          </div>

          <div className="space-y-6">
            <section>
              <h2 className="text-xl font-semibold text-gray-900 mb-3">Database Fix Required</h2>
              <p className="text-gray-700 mb-4">
                The <code className="bg-gray-100 px-2 py-1 rounded text-sm">miHeladera</code> view has been updated to
                handle the new <code className="bg-gray-100 px-2 py-1 rounded text-sm">Eliminado</code> column in the{" "}
                <code className="bg-gray-100 px-2 py-1 rounded text-sm">UsuarioXHeladera</code> table.
              </p>

              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
                <h3 className="font-semibold text-blue-900 mb-2">📁 SQL Scripts to Run (in order):</h3>
                <ol className="list-decimal list-inside space-y-2 text-sm text-blue-800">
                  <li>
                    <code className="bg-white px-2 py-1 rounded">scripts/fix-miHeladera-view-v1.sql</code> - Fix
                    sp_GetProductosByHeladeraId
                  </li>
                  <li>
                    <code className="bg-white px-2 py-1 rounded">scripts/fix-miHeladera-view-v2.sql</code> - Fix
                    getProductosByNombreHeladeraAndIdUsuario
                  </li>
                  <li>
                    <code className="bg-white px-2 py-1 rounded">scripts/fix-miHeladera-view-v3.sql</code> - Fix
                    traerNombresHeladerasById
                  </li>
                  <li>
                    <code className="bg-white px-2 py-1 rounded">scripts/fix-miHeladera-view-v4.sql</code> - Fix
                    SeleccionarHeladeraByNombre
                  </li>
                  <li>
                    <code className="bg-white px-2 py-1 rounded">scripts/fix-miHeladera-view-v5.sql</code> - Fix
                    BuscarHeladeras
                  </li>
                </ol>
              </div>
            </section>

            <section>
              <h2 className="text-xl font-semibold text-gray-900 mb-3">What Was Fixed</h2>
              <div className="bg-gray-50 rounded-lg p-4 space-y-3 text-sm">
                <div className="flex items-start gap-3">
                  <span className="text-green-500 font-bold">✓</span>
                  <div>
                    <strong>Soft Delete Filtering:</strong> All stored procedures now filter out records where{" "}
                    <code className="bg-white px-2 py-1 rounded text-xs">UsuarioXHeladera.Eliminado = 1</code>
                  </div>
                </div>
                <div className="flex items-start gap-3">
                  <span className="text-green-500 font-bold">✓</span>
                  <div>
                    <strong>Product Display:</strong> Only products from active user-heladera relationships are shown
                  </div>
                </div>
                <div className="flex items-start gap-3">
                  <span className="text-green-500 font-bold">✓</span>
                  <div>
                    <strong>Heladera Selection:</strong> Users can only access heladeras they have active access to
                  </div>
                </div>
                <div className="flex items-start gap-3">
                  <span className="text-green-500 font-bold">✓</span>
                  <div>
                    <strong>Data Integrity:</strong> Prevents displaying products from deleted or archived heladeras
                  </div>
                </div>
              </div>
            </section>

            <section>
              <h2 className="text-xl font-semibold text-gray-900 mb-3">How to Run the Scripts</h2>
              <div className="bg-gray-50 rounded-lg p-4">
                <ol className="list-decimal list-inside space-y-2 text-sm text-gray-700">
                  <li>Open SQL Server Management Studio (SSMS)</li>
                  <li>Connect to your Keepi_DataBase</li>
                  <li>Open each SQL script file in order (v1 through v5)</li>
                  <li>Execute each script by pressing F5 or clicking Execute</li>
                  <li>Verify that each stored procedure is created successfully</li>
                  <li>Test the miHeladera view in your application</li>
                </ol>
              </div>
            </section>

            <section className="border-t pt-6">
              <h2 className="text-xl font-semibold text-gray-900 mb-3">Project Structure</h2>
              <div className="grid grid-cols-2 gap-4 text-sm">
                <div>
                  <h3 className="font-semibold text-gray-700 mb-2">Controllers</h3>
                  <ul className="space-y-1 text-gray-600">
                    <li>• HeladeraController.cs</li>
                    <li>• HomeController.cs</li>
                    <li>• AuthController.cs</li>
                    <li>• ChatController.cs</li>
                    <li>• RecetasController.cs</li>
                  </ul>
                </div>
                <div>
                  <h3 className="font-semibold text-gray-700 mb-2">Models</h3>
                  <ul className="space-y-1 text-gray-600">
                    <li>• Heladera.cs</li>
                    <li>• ProductoXHeladera.cs</li>
                    <li>• UsuarioXHeladera.cs</li>
                    <li>• BD.cs (Database Layer)</li>
                  </ul>
                </div>
              </div>
            </section>
          </div>
        </div>

        <div className="bg-green-600 text-white rounded-lg p-6 text-center">
          <h3 className="text-lg font-semibold mb-2">✅ Solution Ready</h3>
          <p className="text-green-100">
            Download the ZIP file and run the SQL scripts to fix the miHeladera view issue!
          </p>
        </div>
      </div>
    </div>
  )
}
