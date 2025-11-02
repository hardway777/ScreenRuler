// Copyright Rising Sun. All Rights Reserved.

#pragma once

#include "Tasks/VehicleAITaskBase.h"
#include "MassActorSubsystem.h"
#include "TaskStopVehicle.generated.h"

namespace UE::MassBehavior { struct FStateTreeDependencyBuilder; }
struct FMassTrafficPIDVehicleControlFragment;

USTRUCT()
struct FTaskStopVehicleInstanceData : public FVehicleAITaskBaseInstanceData
{
    GENERATED_BODY()

    /** Сила, с которой будет применяться тормоз (от 0.0 до 1.0). */
    UPROPERTY(EditAnywhere, Category = "Parameters", meta = (ClampMin = "0.0", ClampMax = "1.0"))
    float BrakeForce = 1.0f;

    /** Скорость (в см/с), ниже которой машина считается остановившейся. */
    UPROPERTY(EditAnywhere, Category = "Parameters", meta = (ClampMin = "0.0"))
    float StopSpeedThreshold = 5.0f;

    /** Время в секундах, которое машина будет ждать после полной остановки. */
    UPROPERTY(EditAnywhere, Category = "Parameters", meta = (ClampMin = "0.0"))
    float WaitDuration = 1.0f;

    // Таймер для отсчета времени ожидания
    float TimeRemaining = 0.0f;
};

/**
 * Задача, которая останавливает машину, ждет указанное время и завершается успехом.
 */
USTRUCT(meta=(DisplayName="Stop Vehicle"))
struct FTaskStopVehicle : public FVehicleAITaskBase
{
    GENERATED_BODY()

    using FInstanceDataType = FTaskStopVehicleInstanceData;
    FTaskStopVehicle();

protected:
    virtual bool Link(FStateTreeLinker& Linker) override;
    virtual void GetDependencies(UE::MassBehavior::FStateTreeDependencyBuilder& Builder) const override;
    virtual const UStruct* GetInstanceDataType() const override;
    virtual EStateTreeRunStatus EnterState(FStateTreeExecutionContext& Context, const FStateTreeTransitionResult& Transition) const override;
    virtual EStateTreeRunStatus Tick(FStateTreeExecutionContext& Context, const float DeltaTime) const override;
    virtual void ExitState(FStateTreeExecutionContext& Context, const FStateTreeTransitionResult& Transition) const override;

    TStateTreeExternalDataHandle<FMassTrafficPIDVehicleControlFragment> TrafficControlHandle;
    TStateTreeExternalDataHandle<FMassActorFragment> ActorFragmentHandle;
};