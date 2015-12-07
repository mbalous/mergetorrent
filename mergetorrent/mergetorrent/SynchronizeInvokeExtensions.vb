Module SynchronizeInvokeExtensions
    Public Delegate Function GenericLambdaFunctionWithParam (Of TInputType, TOutputType)(input As TInputType) As _
        TOutputType

    Private Delegate Function InvokeLambdaFunctionCallback (Of TInputType, TOutputType) _
        (f As GenericLambdaFunctionWithParam(Of TInputType, TOutputType), input As TInputType,
         c As System.ComponentModel.ISynchronizeInvoke) As TOutputType

    Public Function InvokeEx (Of TInputType, TOutputType)(
                                                        f As _
                                                           GenericLambdaFunctionWithParam(Of TInputType, TOutputType),
                                                        input As TInputType,
                                                        c As System.ComponentModel.ISynchronizeInvoke) _
        As TOutputType
        If c.InvokeRequired Then
            Dim d As New InvokeLambdaFunctionCallback(Of TInputType, TOutputType)(AddressOf InvokeEx)
            Return DirectCast(c.Invoke(d, New Object() {f, input, c}), TOutputType)
        Else
            Return f(input)
        End If
    End Function

    Public Delegate Sub GenericLambdaSubWithParam (Of InputType)(input As InputType)

    Public Sub InvokeEx (Of TInputType)(s As GenericLambdaSubWithParam(Of TInputType), input As TInputType,
                                       c As System.ComponentModel.ISynchronizeInvoke)
        InvokeEx (Of TInputType, Object)(Function(i As TInputType) As Object
            s(i)
            Return Nothing
                                           End Function, input, c)
    End Sub

    Public Delegate Sub GenericLambdaSub()

    Public Sub InvokeEx(s As GenericLambdaSub, c As System.ComponentModel.ISynchronizeInvoke)
        InvokeEx (Of Object, Object)(Function(i As Object) As Object
            s()
            Return Nothing
                                        End Function, Nothing, c)
    End Sub

    Public Delegate Function GenericLambdaFunction (Of OutputType)() As OutputType

    Public Function InvokeEx (Of TOutputType)(f As GenericLambdaFunction(Of TOutputType),
                                             c As System.ComponentModel.ISynchronizeInvoke) As TOutputType
        Return InvokeEx (Of Object, TOutputType)(Function(i As Object) f(), Nothing, c)
    End Function
End Module
